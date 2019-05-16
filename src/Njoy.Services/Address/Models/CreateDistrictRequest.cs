using FluentValidation;

namespace Njoy.Services
{
    public sealed class CreateDistrictRequest : AbstractValidator<CreateDistrictRequest>
    {
        public string Name { get; set; }
        public int CountyId { get; set; }

        public CreateDistrictRequest()
        {
            RuleFor(o => o.Name).NotEmpty();
            RuleFor(o => o.CountyId).NotEmpty();
        }
    }
}