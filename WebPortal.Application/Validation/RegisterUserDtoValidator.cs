using FluentValidation;
using Microsoft.AspNetCore.Http;
using WebPortal.Application.Dtos.User;
using WebPortal.Domain.User;

namespace WebPortal.Application.Validation;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(dto => dto.Email).EmailAddress();
        RuleFor(dto => dto.Password).MinimumLength(8).MaximumLength(20);
        RuleFor(dto => dto.NickName).Must(s => s.StartsWith("@"));
        RuleFor(dto => dto.Avatar).Must(new ImageValidator().Validate).When(dto => dto.Avatar != null);
    }
}