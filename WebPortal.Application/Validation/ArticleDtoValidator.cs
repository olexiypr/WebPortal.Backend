using FluentValidation;
using WebPortal.Application.Dtos.Article;
using WebPortal.Domain.Enums;

namespace WebPortal.Application.Validation;

public class ArticleDtoValidator : AbstractValidator<CreateArticleDto>
{
    public ArticleDtoValidator()
    {
        RuleFor(dto => dto.Status).Must(statuses =>
            statuses is ArticleStatuses.Published or ArticleStatuses.InDraft);
        RuleFor(dto => dto.Text).NotNull().NotEmpty().When(dto => dto.Status == ArticleStatuses.Published);
    }    
}