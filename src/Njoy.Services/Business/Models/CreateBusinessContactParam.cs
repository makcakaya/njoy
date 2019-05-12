using FluentValidation;

namespace Njoy.Services
{
    public sealed class CreateBusinessContactParam : AbstractValidator<CreateBusinessContactParam>
    {
        public string Phone { get; set; }
        public string Email { get; set; }

        public CreateBusinessContactParam()
        {
            RuleFor(o => o.Email).EmailAddress().Unless(o => string.IsNullOrWhiteSpace(o.Email));
        }
    }
}