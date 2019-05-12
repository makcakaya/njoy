using FluentValidation;
using Nensure;
using Njoy.Data;
using System.Threading.Tasks;

namespace Njoy.Services
{
    public sealed class BusinessService : IBusinessService
    {
        private readonly NjoyContext _context;

        public BusinessService(NjoyContext context)
        {
            Ensure.NotNull(context);
            _context = context;
        }

        public async Task<Business> Create(CreateBusinessParam createBusiness)
        {
            Ensure.NotNull(createBusiness);
            await createBusiness.ValidateAndThrowAsync(createBusiness);

            var business = new Business
            {
                Name = createBusiness.Name,
            };
            await _context.Set<Business>().AddAsync(business);
            await _context.SaveChangesAsync();

            if (createBusiness.Address != null)
            {
                createBusiness.Address.BusinessId = business.Id;
                await CreateAddress(createBusiness.Address);
            }
            return business;
        }

        public async Task<BusinessAddress> CreateAddress(CreateBusinessAddressParam createAddress)
        {
            Ensure.NotNull(createAddress);
            createAddress.ValidateAndThrow(createAddress);

            var address = new BusinessAddress
            {
                BusinessId = createAddress.BusinessId,
                DistrictId = createAddress.DistrictId,
                PostalCode = createAddress.PostalCode,
                StreetAddress = createAddress.StreetAddress,
            };
            await _context.Set<BusinessAddress>().AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }
}