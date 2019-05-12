using Nensure;
using PhoneNumbers;

namespace Njoy.Services
{
    public sealed class GooglePhoneNumberUtility : IPhoneNumberValidator, IPhoneNumberFormatter
    {
        private const string DefaultRegion = "TR";
        private readonly PhoneNumberUtil _util;

        public GooglePhoneNumberUtility(PhoneNumberUtil util)
        {
            Ensure.NotNull(util);
            _util = util;
        }

        public bool IsValid(string phoneNumber, string defaultRegion = null)
        {
            Ensure.NotNull(phoneNumber);
            if (TryParse(phoneNumber, defaultRegion ?? DefaultRegion, out var parsed))
            {
                return _util.IsValidNumber(parsed);
            }
            return false;
        }

        public string Format(string phoneNumber, string defaultRegion = null)
        {
            if (!TryParse(phoneNumber, defaultRegion ?? DefaultRegion, out var parsed))
            {
                throw new AssertionException($"{phoneNumber} is not a parseable phone number.");
            }
            return _util.IsValidNumber(parsed) ?
                _util.Format(parsed, PhoneNumberFormat.E164) :
                throw new AssertionException($"{phoneNumber} is not a valid phone number.");
        }

        private bool TryParse(string phoneNumber, string defaultRegion, out PhoneNumber parsedPhoneNumber)
        {
            try
            {
                parsedPhoneNumber = _util.Parse(phoneNumber, defaultRegion);
            }
            catch (NumberParseException)
            {
                parsedPhoneNumber = null;
            }
            return parsedPhoneNumber != null;
        }
    }
}