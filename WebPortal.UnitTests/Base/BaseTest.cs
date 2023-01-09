using AutoFixture;
using AutoMapper;
using WebPortal.Application.Mapping;

namespace WebPortal.UnitTests.Base;

public class BaseTest
{
    protected Fixture Fixture { get; set; }
    protected IMapper Mapper { get; set; }

    public BaseTest()
    {
        Fixture = new Fixture();
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        Mapper = new Mapper(new MapperConfiguration(opt =>
        {
            opt.AddProfiles(new List<Profile>
            {
                new ArticleMapper(),
                new CommentaryMapper(),
                new SearchMapper(),
                new TagMapper(),
                new UserMapper(),
                new ArticleCategoryMapper()
            });
        }));
    }
}