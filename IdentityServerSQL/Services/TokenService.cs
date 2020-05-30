using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityServerSQL.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServerSQL.Services
{
    public static class TokenService
    {
        public static string GenerateToken(ApplicationUser user, List<ApplicationRole> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            List<Claim> userRoles = new List<Claim>();
            
            //adicionar roles no JWT 
            foreach (var role in roles)
            {
                userRoles.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.UserName),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Sid, user.ApplicationUserId.ToString()),
                    new Claim(ClaimTypes.Expiration,DateTime.Now.AddHours(HashingOptions.ExpirationInHours).ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(HashingOptions.ExpirationInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            tokenDescriptor.Subject.AddClaims(userRoles);
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}