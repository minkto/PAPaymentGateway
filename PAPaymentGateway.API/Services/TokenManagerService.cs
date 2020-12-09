using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PAPaymentGateway.Core.Interfaces;
using PAPaymentGateway.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PAPaymentGateway.API.Services
{
    /// <summary>
    /// This service will manage the Tokens to be used for the
    /// API.
    /// </summary>
    public class TokenManagerService : ITokenManagerService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentitySettings _identitySettings;

        public TokenManagerService(UserManager<IdentityUser> userManager, 
            IOptions<IdentitySettings> identitySettings)
        {
            _userManager = userManager;
            _identitySettings = identitySettings.Value;
        }

        /// <summary>
        /// This will create and return a newly created JWT(JSON Web Token) which
        /// will allow this user to be authenticated, and authorized to utelize 
        /// functionality for the API.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>JWT Token</returns>
        public async Task<string> CreateToken(string username)
        {
            // Get the user
            var user = await _userManager.FindByNameAsync(username);

            // Get their roles.
            var roles = await _userManager.GetRolesAsync(user);

            // Get the Claims for the Token, of this authenticated user, and
            // add to the payload.
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            foreach (string r in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_identitySettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
