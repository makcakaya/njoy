using Microsoft.AspNetCore.Identity;

namespace Njoy.Admin
{
    public sealed class AdminRole : IdentityRole
    {
        public static readonly string Standart = "Standart";
        public static readonly string Root = "Root";
        public static readonly string Sales = "Sales";
    }
}