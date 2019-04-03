using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASPJWTPractice.Db;
using ASPJWTPractice.Identity;
using ASPJWTPractice.Interfaces;
using ASPJWTPractice.Request;
using ASPJWTPractice.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ASPJWTPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserRepository _userRepository;
        private ILogger _logger;
        private ITokenFactory _tokenFactory;
        private IJWTFactory _jwtFactory;

        public AuthController(IServiceProvider serviceProvider)
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _jwtFactory = serviceProvider.GetService<IJWTFactory>();
            _tokenFactory = serviceProvider.GetService<ITokenFactory>();
            _logger = serviceProvider.GetService<ILogger>();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUser loginUser)
        {
            _logger.LogInfo($"{ loginUser.UserName} logging in.");

            if (!ModelState.IsValid)
            {
                _logger.LogInfo("Username & password required");
                return BadRequest(ModelState);
            }

            try
            {
                AppUser user = await _userRepository.FindIdentityUser(loginUser.UserName);

                if (user == null)
                {
                    _logger.LogInfo("Invalid username or password");
                    return BadRequest(new LoginResponse(new[] { new Error { Code = 555, Description = "Invalid username or password" } }));
                }
                else
                {
                    bool passwordValid = await _userRepository.CheckPassword(user, loginUser.Password);

                    if (passwordValid)
                    {
                        string refreshToken = _tokenFactory.GenerateToken();
                        string remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

                        if (remoteIpAddress == null)
                        {
                            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                            {
                                remoteIpAddress = Request.Headers["X-Forwarded-For"];
                            }
                        }

                        User us = _userRepository.FindUser(user);
                        us.isOnline = true;

                        //us.AddRefreshToken(refreshToken, us.Id, remoteIpAddress);
                        await _userRepository.AddRefreshToken(us, refreshToken, us.Id, remoteIpAddress);
                        await _userRepository.Update(us);

                        var accessToken = await _jwtFactory.GenerateEncodedToken(us.IdentityId, us.UserName);
                        _logger.LogInfo("Valid login details");
                        return Ok(new LoginResponse(accessToken, refreshToken));
                    }
                    else
                    {
                        return BadRequest(new LoginResponse(new[] { new Error { Code = 666, Description = "Invalid username or password" } }));
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new LoginResponse(
                    new[] 
                    {
                        new Error { Code = 000, Description = $"Exception: {ex.Message}, Inner exception: {ex.InnerException?.Message}, , stack trace: {ex.StackTrace}" }
                    }));
            }
            
        }

        [HttpPost("refreshtoken")]
        public async Task<ActionResult> RfrshTkn([FromBody] ExchangeRefreshTokenRequest tokens)
        {
            _logger.LogInfo($"refresh token.");

            if (!ModelState.IsValid)
            {
                _logger.LogInfo("All required");
                return BadRequest(ModelState);
            }

            try
            {
                var cp = _jwtFactory.GetPrincipalFromToken(tokens.AccessToken);

                if (cp != null)
                {
                    var id = cp.Claims.First(c => c.Type == "id");
                    var user = await _userRepository.GetByIdentId(id.Value);

                    if (user != null)
                    {
                        string remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

                        if (remoteIpAddress == null)
                        {
                            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                            {
                                remoteIpAddress = Request.Headers["X-Forwarded-For"];
                            }
                        }

                        //bool pred1 = user.HasValidRefreshToken(tokens.RefreshToken);
                        bool hasValidRT = await _userRepository.HasValidRefreshToken(user, tokens.RefreshToken);
                        if (hasValidRT)
                        {
                            var newAccessToken = await _jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName);
                            string newRefreshToken = _tokenFactory.GenerateToken();
                            //user.RemoveRefreshToken(tokens.RefreshToken);
                            //user.AddRefreshToken(newRefreshToken, user.Id, remoteIpAddress);
                            _userRepository.RemoveRefreshToken(user, tokens.RefreshToken);
                            await _userRepository.AddRefreshToken(user, newRefreshToken, user.Id, remoteIpAddress);
                            await _userRepository.Update(user);
                            return Ok(new RefreshTokenResponse(newAccessToken, newRefreshToken));
                        }
                        return BadRequest(new RefreshTokenResponse(new[] { new Error { Code = 777, Description = "Invalid refresh token" } }));
                    }
                    return BadRequest(new RefreshTokenResponse(new[] { new Error { Code = 999, Description = "User no longer exists." } }));
                }
                return BadRequest(new RefreshTokenResponse(new[] { new Error { Code = 888, Description = "Invalid access JWT" } }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RefreshTokenResponse(
                    new[]
                    {
                        new Error { Code = 000, Description = $"Exception: {ex.Message}, Inner exception: {ex.InnerException?.Message}, stack trace: {ex.StackTrace}" }
                    }));
            }

        }
    }
}
