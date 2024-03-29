﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Njoy.Data;

namespace Njoy.Admin
{
    public static class MvcExtensions
    {
        public static void CustomAddMvc(this IServiceCollection services)
        {
            services
                .AddMvc(config =>
                {
                    config.Filters.Add<AdminExceptionFilterAttribute>();
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole(AppRole.AdminRoot, AppRole.AdminStandard)
                        .Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .CustomAddValidation();
            services.CustomAddExceptionHandling();
        }
    }
}