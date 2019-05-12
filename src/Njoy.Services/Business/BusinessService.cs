using FluentValidation;
using Nensure;
using Njoy.Data;

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

        public Business Create(CreateBusinessParam createBusiness)
        {
            Ensure.NotNull(createBusiness);
            createBusiness.ValidateAndThrow(createBusiness);

            using (var transaction = _context.Database.BeginTransaction())
            {
                var business = new Business
                {
                    Name = createBusiness.Name,
                };
                _context.Set<Business>().Add(business);

                var createAddress = createBusiness.Address;
                var address = new BusinessAddress
                {
                    Business = business,
                    DistrictId = createAddress.DistrictId,
                    PostalCode = createAddress.PostalCode,
                    StreetAddress = createAddress.StreetAddress,
                };
                _context.Set<BusinessAddress>().Add(address);

                _context.SaveChanges();
                transaction.Commit();
                return business;
            }
        }
    }
}