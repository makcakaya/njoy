using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class CreateDefaultUserFeature
    {
        private readonly UserManager<AdminUser> _userManager;

        public CreateDefaultUserFeature(UserManager<AdminUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task Run(CreateDefaultUserParam param)
        {
            if (param is null) { throw new ArgumentNullException(nameof(param)); }

            var rootUserExists = (await _userManager.GetUsersInRoleAsync(AdminRole.Root)).Count > 0;
            if (rootUserExists)
            {
                return;
            }

            var user = new AdminUser
            {
                UserName = param.Username
            };

            var createResult = await _userManager.CreateAsync(user, param.Password);
            if (!createResult.Succeeded)
            {
                throw new Exception($"{nameof(CreateDefaultUserFeature)} failed with following errors: {JsonConvert.SerializeObject(createResult.Errors)}");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, AdminRole.Root);
            if (!roleResult.Succeeded)
            {
                throw new Exception($"{nameof(CreateDefaultUserFeature)} failed with following errors: {JsonConvert.SerializeObject(roleResult.Errors)}");
            }
        }
    }
}