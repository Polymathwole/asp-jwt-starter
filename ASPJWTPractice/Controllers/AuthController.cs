using System;
using System.Collections.Generic;
using System.Linq;
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
            AppUser user = await _userRepository.FindIdentityUser(loginUser.UserName);

            if (user == null)
            {
                _logger.LogInfo("Invalid username or password");
                return BadRequest(new { error = "Invalid username or password"});
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
                    us.AddRefreshToken(refreshToken, us.Id, remoteIpAddress);
                    await _userRepository.Update(us);

                    var accessToken = await _jwtFactory.GenerateEncodedToken(us.IdentityId.ToString(), us.UserName);
                    _logger.LogInfo("Valid login details");
                    return Ok(new LoginResponse(accessToken, refreshToken));
                }
                else
                {
                    return Ok(new LoginResponse(new []{ new Error { Code=666, Description = "Invalid username or password" } }));
                }
            }
        }

        [HttpPost("refreshtoken")]
        public async Task<ActionResult> RfrshTkn([FromBody] ExchangeRefreshTokenRequest loginUser)
        {
            _logger.LogInfo($"refresh token.");

            if (!ModelState.IsValid)
            {
                _logger.LogInfo("All required");
                return BadRequest(ModelState);
            }
        }
    }
}
