using Microsoft.EntityFrameworkCore;
using WebPortal.Domain;
using WebPortal.Domain.Enums;
using WebPortal.Domain.User;

namespace WebPortal.Persistence.SeedData;

public static class SeedData
{
    public static void Seed(this ModelBuilder builder)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "max@gmail.com",
            Password = "12345",
            Role = "user",
            NickName = "@maxim4ik",
            Name = "MaxON",
            Description = "I`m MaximON!",
            RegistrationDate = DateTime.Now
        };
        var article = new Article
        {
            Id = Guid.NewGuid(),
            AuthorId = user.Id,
            Name = "First article",
            KeyWords = new List<string> {"example", "article"},
            Text = "This is example article",
            CreationDate = DateTime.Now,
            Status = ArticleStatuses.Published,
        };
        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = "#sport",
        };
        var category = new ArticleCategory
        {
            Id = Guid.NewGuid(),
            Name = "First category",
        };
        var commentary = new Commentary
        {
            Id = Guid.NewGuid(),
            ArticleId = article.Id,
            AuthorId = user.Id,
            Text = "Good article!",
            CreationDate = DateTime.Now,
            CountLikes = 3,
            CountDislikes = 5
        };
        article.CategoryId = category.Id;

        builder.Entity<User>().HasData(user);
        builder.Entity<Article>().HasData(article);
        builder.Entity<ArticleCategory>().HasData(category);
        builder.Entity<Commentary>().HasData(commentary);
        builder.Entity<Tag>().HasData(tag);
    }
}