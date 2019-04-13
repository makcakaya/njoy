using FluentValidation;
using System.Collections.Generic;

namespace Njoy.Services
{
    public sealed class EditUserRequest : AbstractValidator<EditUserRequest>
    {
        public string Id { get; set; }
        public Dictionary<string, string> Claims { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }

        public EditUserRequest()
        {
            RuleFor(x => x.Id).NotEmpty();
            When(x => x.Email != null, () =>
                RuleFor(x => x.Email).EmailAddress());
            When(x => !string.IsNullOrEmpty(x.NewPassword), () =>
            {
                RuleFor(x => x.NewPassword).Equal(x => x.NewPasswordConfirm);
                RuleFor(x => x.CurrentPassword).NotEmpty();
            });
        }
    }
}