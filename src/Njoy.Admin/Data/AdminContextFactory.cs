namespace Njoy.Admin.Data
{
    public sealed class AdminContextFactory : IAdminContextFactory
    {
        public AdminContext Create()
        {
            return new AdminContext();
        }
    }
}