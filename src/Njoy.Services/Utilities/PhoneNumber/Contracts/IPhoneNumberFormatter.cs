namespace Njoy.Services
{
    public interface IPhoneNumberFormatter
    {
        string Format(string phoneNumber, string defaultRegion = null);
    }
}