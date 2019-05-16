using FluentValidation;

namespace Njoy.Services
{
    public sealed class CreateCityRequest : AbstractValidator<CreateCityRequest>
    {
        public string Name { get; set; }
        public int LicensePlateCode { get; set; }

        public CreateCityRequest()
        {
            RuleFor(o => o.Name).NotEmpty();
            RuleFor(o => o.LicensePlateCode).InclusiveBetween(1, 82);
        }
    }
}