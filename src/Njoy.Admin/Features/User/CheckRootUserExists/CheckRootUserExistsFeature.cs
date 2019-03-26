using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Njoy.Admin.Features
{
    public sealed class CheckRootUserExistsFeature : ICheckRootUserExistsFeature
    {
        private readonly UserManager<AdminUser> _userManager;

        public CheckRootUserExistsFeature(UserManager<AdminUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<bool> Run()
        {
            return (await _userManager.GetUsersInRoleAsync(AdminRole.Root)).Count > 0;
        }
    }
}