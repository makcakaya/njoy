using FluentValidation;

namespace Njoy.Services
{
    public sealed class CreateUserRequest : AbstractValidator<CreateUserRequest>, IUserRegistrationModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }

        public CreateUserRequest()
        {
            RuleFor(x => x.Username).MinimumLength(4);
            RuleFor(x => x.Password).Equal(x => x.PasswordConfirm);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Role).NotEmpty();
        }
    }
}