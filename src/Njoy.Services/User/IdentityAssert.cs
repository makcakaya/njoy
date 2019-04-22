using Microsoft.AspNetCore.Identity;
using Nensure;

namespace Njoy.Services
{
    public static class IdentityAssert
    {
        public static void ThrowIfFailed(IdentityResult result, string operation)
        {
            Ensure.NotNull(result);
            if (!result.Succeeded)
            {
                throw new OperationFailedException(operation, result.Errors);
            }
        }
    }
}