using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAndRolesTest.DTOs;
using MultiTenantAndRolesTest.DTOs.Admin;
using MultiTenantAndRolesTest.Helpers;
using MultiTenantAndRolesTest.Interfaces;
using MultiTenantAndRolesTest.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace MultiTenantAndRolesTest.Controllers
{
    [Route("admin")]
    [Authorize]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repo;
        private readonly IUserManager _userManager;
        private readonly ITokenRepository _token;
        public AdminController(IAdminRepository repo, IUserManager userManager, ITokenRepository token)
        {
            _repo = repo;
            _userManager = userManager;
            _token = token;
        }

        [HttpPost("user/register")]
        [SwaggerOperation("Register a new user")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<UserRegisterLoginSuccessDto>))]
        [ValidateModelState]
        public async Task<IActionResult> UserRegister([FromBody] UserRegisterDto userDto)
        {

            try
            {
                //var user = await _userManager.GetOneUserByEmailAsync(userDto.Email);
                var createdUser = await _userManager.CreateUserAsync(userDto);
                var user = await _userManager.GetOneUserByEmailAsync(createdUser.Email);

                return Ok(new UserRegisterLoginSuccessDto
                {
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    Email = createdUser.Email,
                    PhoneNumber = createdUser.PhoneNumber,
                    Token = await _token.CreateToken(user),
                    Roles = createdUser.Roles,

                });
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    throw new Exception("error creating role permissions");
                }
            }
        }
        [HttpPost("user/role")]
        [SwaggerOperation("User Roles create")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Permission>))]
        [ValidateModelState]
        public async Task<IActionResult> UserRoleCreate([FromBody] UserRoleDto userRole)
        {

            try
            {
                var cUserRole = await _repo.CreateUserRoleAsync(userRole);
                return Ok(cUserRole);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    throw new Exception("error creating UserRole");
                }
            }
        }
        [HttpPost("role")]
        [SwaggerOperation("Role create")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Role>))]
        [ValidateModelState]
        public async Task<IActionResult> RoleCreate([FromBody] Role role)
        {

            try
            {
                var cRole = await _repo.CreateRoleAsync(role);
                return Ok(cRole);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    throw new Exception("error creating Role");
                }
            }

        }
        [HttpPost("permission")]
        [SwaggerOperation("Permission create")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Permission>))]
        [ValidateModelState]
        public async Task<IActionResult> PermissionCreate([FromBody] Permission permission)
        {

            try
            {
                var cPermission = await _repo.CreatePermissionAsync(permission);
                return Ok(cPermission);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    throw new Exception("error creating Permission");
                }

            }


        }

        [HttpPost("permission/role")]
        [SwaggerOperation("Permission Roles create")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Permission>))]
        [ValidateModelState]
        public async Task<IActionResult> RolePermissionCreate([FromBody] RolePermissionDto rolePermission)
        {

            try
            {
                var cRolePermission = await _repo.CreateRolePermissionAsync(rolePermission);
                return Ok(cRolePermission);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    throw new Exception("error creating role permissions");
                }
            }
        }

    }
}
