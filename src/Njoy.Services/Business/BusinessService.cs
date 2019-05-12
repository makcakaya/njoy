using Nensure;
using Njoy.Data;
using System;

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
            using (var transaction = _context.Database.BeginTransaction())
            {
                throw new NotImplementedException();
            }
        }
    }
}