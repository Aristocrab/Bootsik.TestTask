using Bootsik.TestTask.WebApi.Dtos;
using FluentValidation;

namespace Bootsik.TestTask.WebApi.Validators;

public class HtmlTemplateDtoValidator : AbstractValidator<HtmlTemplateDto>
{
    public HtmlTemplateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(64);
        RuleFor(x => x.Content).NotEmpty();
    }
}