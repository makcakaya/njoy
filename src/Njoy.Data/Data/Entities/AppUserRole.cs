using Microsoft.AspNetCore.Identity;

namespace Njoy.Data
{
    public sealed class AppRole : IdentityRole
    {
        public const string AdminRoot = "AdminRoot";
        public const string AdminStandard = "AdminStandard";
        public const string Merchant = "Merchant";
    }
}