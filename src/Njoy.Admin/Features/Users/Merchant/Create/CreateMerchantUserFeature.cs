﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Nensure;
using Njoy.Data;
using Njoy.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Njoy.Admin
{
    public sealed class CreateMerchantUserFeature
    {
        public sealed class Handler : IRequestHandler<Request>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;
            private readonly NjoyContext _context;

            public Handler(IUserService userService, IMapper mapper, NjoyContext context)
            {
                Ensure.NotNull(userService).NotNull(mapper).NotNull(context);
                _userService = userService;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                Ensure.NotNull(request);
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    // Create user
                    var mappedRequest = _mapper.Map<CreateUserRequest>(request);
                    mappedRequest.Role = AppRole.Merchant;
                    var createUserResponse = await _userService.Create(mappedRequest);

                    // Create merchant
                    var merchant = new Merchant
                    {
                        User = createUserResponse.User,
                    };
                    _context.Set<Merchant>().Add(merchant);

                    // Assign it to an existing Business
                    var business = _context.Set<Business>().FirstOrDefault(b => b.Id == request.BusinessId);
                    if (business is null)
                    {
                        throw new ArgumentException(nameof(CreateMerchantUserFeature),
                            $"Business with code {request.BusinessId} does not exist.");
                    }
                    merchant.BusinessMerchants.Add(new BusinessMerchant
                    {
                        Merchant = merchant,
                        Business = business
                    });

                    _context.SaveChanges();
                    transaction.Commit();

                    return Unit.Value;
                }
            }
        }

        public sealed class Request : AbstractValidator<Request>, IRequest, IUserRegistrationModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirm { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int BusinessId { get; set; }

            public Request()
            {
                RuleFor(r => r.Username).MinimumLength(UserConfig.MinUsernameLength);
                RuleFor(r => r.Email).EmailAddress();
                RuleFor(r => r.Password).MinimumLength(UserConfig.MinPasswordLength).Equal(r => r.PasswordConfirm);
                RuleFor(r => r.BusinessId).NotEmpty();
            }
        }
    }
}