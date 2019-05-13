using FluentValidation;

namespace Njoy.Services
{
    public class CreateBusinessParam : AbstractValidator<CreateBusinessParam>
    {
        public string Name { get; set; }
        public CreateBusinessContactParam Contact { get; set; }
        public CreateBusinessAddressParam Address { get; set; }

        public CreateBusinessParam()
        {
            RuleFor(o => o.Name).NotEmpty();
            RuleFor(o => o.Contact).Must(oo => oo.Validate(oo).IsValid).When(o => o.Contact != null);
            RuleFor(o => o.Address).Must(oo => oo.Validate(oo).IsValid).When(o => o.Address != null);
        }
    }
}