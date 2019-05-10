using FluentValidation;

namespace Njoy.Services
{
    public sealed class CreateBusinessParam : AbstractValidator<CreateBusinessParam>
    {
        public string Name { get; set; }
        public CreateBusinessContactParam Contact { get; set; }
        public CreateBusinessAddressParam Address { get; set; }

        public CreateBusinessParam()
        {
            RuleFor(o => o.Name).NotEmpty();
        }
    }
}