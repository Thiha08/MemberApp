using FluentValidation;

namespace MemberApp.Web.ViewModels.Validations
{
    public class MemberViewModelValidator : AbstractValidator<MemberViewModel>
    {
        public MemberViewModelValidator()
        {
            RuleFor(member => member.Name).NotEmpty().WithMessage("Name cannot be empty");
        }
    }
}
