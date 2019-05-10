using FluentValidation;
using System;

namespace Njoy.Services
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder, string phoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}