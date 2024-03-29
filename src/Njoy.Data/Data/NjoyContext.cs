﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Njoy.Data
{
    public sealed class NjoyContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public NjoyContext()
        {
        }

        public NjoyContext(DbContextOptions<NjoyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyIdentityConfigurations();
            builder.ApplyConfiguration(new CityConfig());
            builder.ApplyConfiguration(new CountyConfig());
            builder.ApplyConfiguration(new DistrictConfig());
            builder.ApplyConfiguration(new MerchantConfig());
            builder.ApplyConfiguration(new BusinessConfig());
            builder.ApplyConfiguration(new BusinessAddressConfig());
            builder.ApplyConfiguration(new BusinessMerchantConfig());
            builder.ApplyConfiguration(new BusinessSubscriptionConfig());
        }
    }
}