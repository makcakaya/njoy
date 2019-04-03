namespace Njoy.Admin
{
    public interface IJwtService
    {
        string GenerateToken(string username, string password);
    }
}