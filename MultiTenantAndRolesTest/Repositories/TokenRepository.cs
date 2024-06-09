﻿using Microsoft.IdentityModel.Tokens;
using MultiTenantAndRolesTest.Interfaces;
using MultiTenantAndRolesTest.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenantAndRolesTest.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _config;
        private readonly IUserManager _userManager;
        private readonly SymmetricSecurityKey _key;
        public TokenRepository(IConfiguration config, IUserManager userManager)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _userManager = userManager;
        }
        public async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var encryption = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = encryption,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}