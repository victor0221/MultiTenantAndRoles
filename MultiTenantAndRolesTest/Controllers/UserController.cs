using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAndRolesTest.DTOs;
using MultiTenantAndRolesTest.DTOs.Admin;
using MultiTenantAndRolesTest.Helpers;
using MultiTenantAndRolesTest.Interfaces;
using MultiTenantAndRolesTest.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace MultiTenantAndRolesTest.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ITokenRepository _token;
        public UserController(IUserManager userManager, ITokenRepository token)
        {
            _userManager = userManager;
            _token = token;
        }

        [HttpPost("login")]
        [SwaggerOperation("Login")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<UserLoginSuccessDto>))]
        [ValidateModelState]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var user = await _userManager.GetOneUserByEmailAsync(loginDto.Email);;
                if (user == null) throw new Exception("no user found");

                var matchPassword = await _userManager.VerifyPassword(loginDto.Password, user.PasswordHash);
                if (matchPassword == false) throw new Exception("password does not match");

                var userRoles = await _userManager.GetRolesAsync(user);


                if (user != null)
                {
                    return Ok(new UserLoginSuccessDto
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Token = await _token.CreateToken(user),
                        Roles = userRoles
                    });
                }
                else
                {
                    return BadRequest("Erro logging in, contact admin");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
