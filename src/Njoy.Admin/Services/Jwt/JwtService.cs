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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtSettings _settings;

        public JwtService(JwtSettings settings, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            Ensure.NotNull(settings, userManager, signInManager);
            _settings = settings;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> GenerateToken(string username, string password)
        {
            Ensure.NotNullOrWhitespace(username, password);
            var user = await _userManager.FindByNameAsync(username);
            Ensure.NotNull(user);
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);
            Ensure.True(signInResult.Succeeded);

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