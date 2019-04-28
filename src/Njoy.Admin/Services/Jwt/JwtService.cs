using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Nensure;
using Njoy.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class JwtService : IJwtService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettings _settings;

        public JwtService(JwtSettings settings, UserManager<AppUser> userManager)
        {
            Ensure.NotNull(settings).NotNull(userManager);
            _settings = settings;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(string username, string password)
        {
            Ensure.NotNullOrWhitespace(username, password);
            var user = await _userManager.FindByNameAsync(username);
            Ensure.NotNull(user);
            var passwordValid = await _userManager.CheckPasswordAsync(user, password);
            Ensure.True(passwordValid);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = await CreateClaimsIdentity(user),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<ClaimsIdentity> CreateClaimsIdentity(AppUser user)
        {
            Ensure.NotNull(user);
            var claims = new List<Claim>(await _userManager.GetClaimsAsync(user));
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));

            return new ClaimsIdentity(claims);
        }
    }
}