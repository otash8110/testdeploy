using FluentValidation;
using Application.DTO;

namespace Application.Services.ValidationsServices
{
    public class ProjectTaskValidator : AbstractValidator<ProjectTaskDto>
    {
        public ProjectTaskValidator()
        {
            RuleFor(t => t.Name).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(100);
            RuleFor(t => t.Description).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(2000);
            RuleFor(t => t.Status).Cascade(CascadeMode.Stop)
                .NotNull()
                .IsInEnum();
            RuleFor(t => t.CreatorId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull();
        }
    }
}
