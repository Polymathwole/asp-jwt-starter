using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ASPJWTPractice.Request;
using Microsoft.AspNetCore.Identity;
using ASPJWTPractice.Interfaces;

namespace ASPJWTPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IUserRepository userStore;
        private ILogger _logger;

        public AccountsController(IServiceProvider serviceProvider)
        {
            userStore = serviceProvider.GetService<IUserRepository>();
            _logger = serviceProvider.GetService<ILogger>();
        }

        [HttpPost("signup")]
        public async Task<ActionResult> Signup([FromBody]SignupUser newUser)
        {
            _logger.LogInfo($"User signup initiated. Username: {newUser.UserName}");
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("User signup. ModelState invalid. A validation condition is not met.");
                return BadRequest(ModelState);
            }

            if (!newUser.Sex.Trim().ToUpper().Equals("M") && !newUser.Sex.ToUpper().Trim().Equals("F"))
            {
                // return BadRequest(new { error = "Sex must be M or F." });
                _logger.LogInfo("Sex must be M or F.");
                return BadRequest(new {
                    errors = new {
                        sex = new string[] { "Sex must be M or F." }
                    }
                });
            }

            try
            {
                IdentityResult identityResult = await userStore.CreateNewUser(newUser);

                if (identityResult.Succeeded)
                {
                    _logger.LogInfo($"User created. Username: {newUser.UserName}. User: {newUser}");
                    return Ok(new
                    {
                        status = StatusCodes.Status200OK,
                        message = "User successfully created."
                    });
                }
                else
                {
                    IEnumerable<IdentityError> errs = identityResult.Errors;
                    _logger.LogInfo($"{@errs}");
                    return BadRequest(errs);
                }
            }
            catch (Exception ex)
            {
                /* return BadRequest(new {
                    statusCode = StatusCodes.Status500InternalServerError,
                    errorMessage = ex.Message
                });*/
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}