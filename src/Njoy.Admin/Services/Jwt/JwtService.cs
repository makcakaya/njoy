using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<string> GenerateToken(string username, string password)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);

            var user = await _userManager.FindByNameAsync(username);
            if (user is null) { throw new InvalidOperationException($"Username {username} does not exist."); }

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
            var claims = new List<Claim>(await _userManager.GetClaimsAsync(user));
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));

            return new ClaimsIdentity(claims);
        }
    }
}