using FluentValidation.Resources;
using FluentValidation.Validators;
using PhoneNumbers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Njoy.Services
{
    public sealed class PhoneNumberValidator : PropertyValidator
    {
        public PhoneNumberValidator(IStringSource errorMessageSource) : base(errorMessageSource)
        {
        }

        public PhoneNumberValidator(string errorMessage) : base(errorMessage)
        {
        }

        public PhoneNumberValidator(string errorMessageResourceName, Type errorMessageResourceType) : base(errorMessageResourceName, errorMessageResourceType)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var phoneNumber = context.PropertyValue as string;
            if (phoneNumber is null)
            {
                throw new ValidationException("Value is not set.");
            }
            var number = PhoneNumberUtil.GetInstance().Parse(phoneNumber, "TR");
            return PhoneNumberUtil.GetInstance().IsValidNumber(number);
        }
    }
}