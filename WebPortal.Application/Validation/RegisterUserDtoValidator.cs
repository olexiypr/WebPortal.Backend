using FluentValidation;
using Microsoft.AspNetCore.Http;
using WebPortal.Application.Dtos.User;
using WebPortal.Domain.User;

namespace WebPortal.Application.Validation;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(dto => dto.Email).EmailAddress();/*Matches(@"\G([a-zA-Z0-9]+)@[\w]{1,7}.");*/
        RuleFor(dto => dto.Password).MinimumLength(8).MaximumLength(20);
        RuleFor(dto => dto.NickName).Must(s => s.StartsWith("@"));
        RuleFor(dto => dto.Avatar).Must(ValidateImage).When(dto => dto.Avatar != null);
    }

    private bool ValidateImage(IFormFile avatar)
    {
        return true;
    }
}