using Microsoft.AspNetCore.Identity;

namespace Njoy.Admin
{
    public sealed class AdminRole : IdentityRole
    {
        public const string Standart = "Standart";
        public const string Root = "Root";
        public const string Sales = "Sales";
    }
}