using FluentValidation;

namespace Njoy.Services
{
    public sealed class CreateBusinessAddressParam : AbstractValidator<CreateBusinessAddressParam>
    {
        public int BusinessId { get; set; }
        public int DistrictId { get; set; }
        public string PostalCode { get; set; }
        public string StreetAddress { get; set; }

        public CreateBusinessAddressParam()
        {
            RuleFor(e => e.DistrictId).NotEmpty();
            RuleFor(e => e.PostalCode).NotEmpty();
            RuleFor(e => e.StreetAddress).NotEmpty();
        }
    }
}