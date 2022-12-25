using FluentValidation;
using WebPortal.Application.Dtos.Commentary;

namespace WebPortal.Application.Validation;

public class AddCommentaryDtoValidator : AbstractValidator<AddCommentaryDto>
{
    public AddCommentaryDtoValidator()
    {
        RuleFor(dto => dto.Text).MinimumLength(1);
    }
}