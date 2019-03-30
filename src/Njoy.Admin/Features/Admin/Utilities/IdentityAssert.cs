using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;

namespace Njoy.Admin.Features
{
    public static class IdentityAssert
    {
        public static void ThrowIfFailed(IdentityResult result, string operation)
        {
            result = result ?? throw new ArgumentNullException(nameof(result));
            operation = operation ?? throw new ArgumentNullException(nameof(operation));

            if (!result.Succeeded)
            {
                throw new Exception($"{operation} failed. Errors: {JsonConvert.SerializeObject(result.Errors)}");
            }
        }
    }
}