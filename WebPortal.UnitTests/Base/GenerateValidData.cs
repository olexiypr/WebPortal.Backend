using AutoFixture;
using WebPortal.Domain;
using WebPortal.Domain.User;

namespace WebPortal.UnitTests.Base;

public class GenerateValidData : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<User>(composer =>
        {
            var id = Guid.NewGuid();
            composer.With(user => user.Id, id);
            var articles = fixture.CreateMany<Article>().ToList();
            articles.ForEach(article => article.AuthorId = article.AuthorId = id);
            composer.With(u => u.Articles, articles);
            return composer;
        });
    }
}