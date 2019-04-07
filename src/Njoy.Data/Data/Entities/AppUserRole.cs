using Microsoft.AspNetCore.Identity;

namespace Njoy.Data
{
    public sealed class AppRole : IdentityRole
    {
        public const string AdminStandart = "Standart";
        public const string AdminRoot = "Root";
        public const string Sales = "Sales";
    }
}