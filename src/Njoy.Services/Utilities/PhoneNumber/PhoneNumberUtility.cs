using System;

namespace Njoy.Services
{
    public sealed class PhoneNumberUtility
    {
        private readonly static Lazy<GooglePhoneNumberUtility> _instance =
            new Lazy<GooglePhoneNumberUtility>(() => new GooglePhoneNumberUtility(PhoneNumbers.PhoneNumberUtil.GetInstance()));

        public static IPhoneNumberFormatter Formatter { get { return _instance.Value; } }
        public static IPhoneNumberValidator Validator { get { return _instance.Value; } }
    }
}