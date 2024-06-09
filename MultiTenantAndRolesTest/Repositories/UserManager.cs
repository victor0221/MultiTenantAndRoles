﻿using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MultiTenantAndRolesTest.Data;
using MultiTenantAndRolesTest.DTOs;
using MultiTenantAndRolesTest.Interfaces;
using MultiTenantAndRolesTest.Models;
using System.Text.RegularExpressions;

namespace MultiTenantAndRolesTest.Repositories
{
    public class UserManager : IUserManager
    {
        private readonly AppDbContext _context;
        public UserManager(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> CheckPasswordPolicy(string password)
        {
            string errorString = "";

            //length test
            if (password.Length < 12) 
                errorString += " Password Must Be 12 Charecters Long";

            // checks for min 1 digit
            if (!Regex.IsMatch(password, @"\d"))
                errorString += " Password must have 1 digit";

            //checks for 1 upper case
            if (!Regex.IsMatch(password, @"[A-Z]"))
                errorString += " Password must have 1 uppercase";

            // checks for 1 special chatecter
            if (!Regex.IsMatch(password, @"[@#$£!_]"))
                errorString += " Password must have 1 special charecter e.g. '@','#','$','£','!' or '_'";
            return errorString;
        }

        public async Task<string> HashPassword(string password)
        {

            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            return hash;
        }

        public async Task<bool> VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        public async Task<List<Role>> UserRolesGetAsync(User user)
        {
            var roles = await _context.UserRole.Where(r => r.UserId == user.Id).Select(ur => ur.Role).ToListAsync();
            return roles;

        }
        public async Task<UserRegisterLoginSuccessDto> UserCreateAsync(UserRegisterDto user)
        {
            try
            {
                string passwordPolicyError = await CheckPasswordPolicy(user.Password);
                if (!string.IsNullOrEmpty(passwordPolicyError))
                {
                    throw new ArgumentException(passwordPolicyError);
                }
                var hashPassword = await HashPassword(user.Password);

                var roleIds = new List<int>();
                foreach (var role in user.Roles)
                {
                    var existingRole = await _context.Role.FirstOrDefaultAsync(r => r.Name == role.Name);
                    if (existingRole == null)
                    {
                        throw new ArgumentException($"Role '{role.Name}' does not exist.");
                    }
                    roleIds.Add(existingRole.Id);
                }

                var newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    PasswordHash = hashPassword,
                };

                await _context.User.AddAsync(newUser);

                await _context.SaveChangesAsync();
                var userRoles = roleIds.Select(roleId => new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = roleId
                });

                await _context.UserRole.AddRangeAsync(userRoles);
                await _context.SaveChangesAsync();
                var addedUser = await _context.User
                    .Where(u => u.Email == newUser.Email)
                    .Select(u => new UserRegisterLoginSuccessDto
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        Roles = _context.UserRole
                                    .Where(ur => ur.UserId == u.Id)
                                    .Select(ur => ur.Role)
                                    .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (addedUser == null)
                {
                    throw new InvalidOperationException("An error occurred while retrieving the user.");
                }

                return addedUser;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innerException && (innerException.Number == 2601 || innerException.Number == 2627))
                {
                    throw new ArgumentException("User with this email already exists.");
                }
                else
                {
                    throw new Exception("An error occurred while saving the entity changes. See the inner exception for details.", ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }


        public async Task<User> UserGetByEmailAsync(string userEmail)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                throw new Exception("not a valid user");
            }
            return user;
        }
        public async Task<bool> UserDelete(int userId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

             _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<UserUpdateSuccess> UserUpdateAsync(int userId, UserUpdateDto userUpdatedInfo)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FirstName = userUpdatedInfo.FirstName ?? user.FirstName;
            user.LastName = userUpdatedInfo.LastName ?? user.LastName;
            user.Email = userUpdatedInfo.Email ?? user.Email;
            user.PhoneNumber = userUpdatedInfo.PhoneNumber ?? user.PhoneNumber;

            if (!string.IsNullOrEmpty(userUpdatedInfo.Password))
            {
                var passCheck = await CheckPasswordPolicy(userUpdatedInfo.Password);
                if (passCheck.Length > 0) throw new Exception(passCheck);
                user.PasswordHash = await HashPassword(userUpdatedInfo.Password);
            }

            if (userUpdatedInfo.Roles != null && userUpdatedInfo.Roles.Any())
            {
                var userRolesToRemove = await _context.UserRole.Where(ur => ur.UserId == userId).ToListAsync();
                _context.UserRole.RemoveRange(userRolesToRemove);

                foreach (var role in userUpdatedInfo.Roles)
                {
                    var existingRole = await _context.Role.FirstOrDefaultAsync(r => r.Name == role.Name);
                    if (existingRole == null)
                    {
                        throw new ArgumentException($"Role '{role.Name}' does not exist.");
                    }
                    var userRole = new UserRole
                    {
                        UserId = userId,
                        RoleId = existingRole.Id
                    };
                    _context.UserRole.Add(userRole);
                }
            }

            await _context.SaveChangesAsync();

            return new UserUpdateSuccess
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = userUpdatedInfo.Roles,
            };
        }

        public async Task<UserUpdateSuccess> UserGetByIdAsync(int userId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var roles = await UserRolesGetAsync(user);
            return new UserUpdateSuccess
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles
            };
        }

    }
}
