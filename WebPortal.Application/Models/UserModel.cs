using System.ComponentModel;
using WebPortal.Application.Models.Article;

namespace WebPortal.Application.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string NickName { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Avatar { get; set; }
    public IEnumerable<ArticlePreviewModel>? Articles { get; set; }
}