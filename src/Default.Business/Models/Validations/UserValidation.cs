using FluentValidation;

namespace Default.Business.Models.Validations
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(a => a.Email).EmailAddress();
        }
    }
}
