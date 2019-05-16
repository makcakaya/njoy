using FluentValidation;

namespace Njoy.Services
{
    public sealed class CreateCountyRequest : AbstractValidator<CreateCountyRequest>
    {
        public int CityId { get; set; }
        public string Name { get; set; }

        public CreateCountyRequest()
        {
            RuleFor(o => o.CityId).NotEmpty();
            RuleFor(o => o.Name).NotEmpty();
        }
    }
}