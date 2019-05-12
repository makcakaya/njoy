namespace Njoy.Services
{
    public interface IPhoneNumberValidator
    {
        bool IsValid(string phoneNumber, string defaultRegion);
    }
}