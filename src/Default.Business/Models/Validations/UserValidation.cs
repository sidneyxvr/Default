using FluentValidation;

namespace Default.Business.Models.Validations
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(user => user.Email).EmailAddress();

            RuleFor(user => user.Name).NotEmpty();
        }
    }
}
