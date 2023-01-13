using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using MockQueryable.Moq;
using Services.Implementation;
using Services.Interfaces;
using Services.Interfaces.Cache;
using WebPortal.Application.Dtos.Article;
using WebPortal.Application.Models.Article;
using WebPortal.Domain;
using WebPortal.Domain.Enums;
using WebPortal.Domain.User;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;
using WebPortal.UnitTests.Base;

namespace WebPortal.UnitTests;

public class ArticleServiceTest : BaseTest
{
    private readonly ArticleService _articleService;
    private readonly Mock<IRepository<Article>> _articleRepository;
    private readonly Mock<IRepository<User>> _userRepository;
    private readonly Mock<IMemoryCache> _memoryCache;
    private readonly Mock<IRepository<ArticleCategory>> _categoryRepository;
    private readonly Mock<ITagService> _tagService;
    private readonly Mock<IPaginationService> _paginationService;
    private readonly Mock<IHttpContextAccessor> _contextAccessor;
    private readonly Mock<ICacheService> _cacheService;
    public ArticleServiceTest()
    {
        _cacheService = new Mock<ICacheService>();
        _articleRepository = new Mock<IRepository<Article>>();
        _userRepository = new Mock<IRepository<User>>();
        _memoryCache = new Mock<IMemoryCache>();
        _categoryRepository = new Mock<IRepository<ArticleCategory>>();
        _tagService = new Mock<ITagService>();
        _paginationService = new Mock<IPaginationService>();
        _contextAccessor = new Mock<IHttpContextAccessor>();
        _articleService = new ArticleService(
            _articleRepository.Object, 
            _userRepository.Object,
            _categoryRepository.Object,
            Mapper,
            _contextAccessor.Object,
            _tagService.Object,
            _paginationService.Object,
            _cacheService.Object);
        
    }
    [Theory, AutoEntityData]
    public async Task GetArticleByIdAsync_WhenArticleExists_Returns_Article(Article article)
    {
        _articleRepository.Setup(repository => repository.Query())
            .Returns( new[] {article}.AsQueryable().BuildMock());
        var id = article.Id;
        var articleModel = Mapper.Map<ArticleModel>(article);
        articleModel.CountViews++;
        var result = await _articleService.GetArticleByIdAsync(id);
        result.Should().BeEquivalentTo(articleModel);
    }

    [Theory, AutoEntityData]
    public async Task GetArticleByIdAsync_WhenArticleNotExists_Throws_Error(Article article, Guid id)
    {
        _articleRepository.Setup(repository => repository.Query())
            .Returns(new [] {article}.AsQueryable().BuildMock());

        Func<Task> action = () => _articleService.GetArticleByIdAsync(id);
        
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Theory, AutoEntityData]
    public async Task GetUserArticlesAsync_Returns_Articles(User user, [Range(0, 2)]ArticleStatuses status)
    {
        var claims = new List<Claim>() {new (ClaimTypes.NameIdentifier, user.Id.ToString())};
        var articles = Fixture.Build<Article>()
            .With(article => article.AuthorId, user.Id)
            .With(article => article.Status, status)
            .CreateMany().ToList();
        _contextAccessor.Setup(accessor => accessor.HttpContext.User.Claims)
            .Returns(claims);
        _articleRepository.Setup(repository => repository.Query())
            .Returns(articles.AsQueryable().BuildMock());
        _paginationService.Setup(service => service.GetArticlesByPagination(articles, null))
            .Returns(articles);
        user.Articles = articles as ICollection<Article>;
        var expected = Mapper.ProjectTo<UserArticlePreviewModel>(articles.AsQueryable());
        
        var actual =
            await _articleService.GetUserArticlesAsync(status, null);
        
        actual.Should().BeEquivalentTo(expected);
    }
    /*[Theory]
    [AutoEntityData]
    public async Task GetPopularArticlesAsync_Returns_Articles([Range(1, 3)]Periods period, IEnumerable<Article> articles)
    {
        _articleRepository.Setup(repository => repository.Query())
            .Returns(articles.AsQueryable());
        _paginationService.Setup(service => service.GetArticlesByPagination(articles, null))
            .Returns(articles);
        var actual = await _articleService.GetPopularArticlesAsync(period.ToString(), null);
    }

    [Theory, AutoEntityData]
    public async Task GetPopularArticlesAsync_Returns_Articles_From_Cache()
    {
        
    }*/

    [Theory, AutoEntityData]
    public async Task CreateArticleAsync_WhenArticleDtoIsValid_ReturnsArticle(User user, IEnumerable<ArticleCategory> category)
    {
        var createArticleDto = Fixture.Build<CreateArticleDto>()
            .With(dto => dto.CategoryId, category.First().Id)
            .Create();
        var claims = new List<Claim>() {new (ClaimTypes.NameIdentifier, user.Id.ToString())};
        var article = Mapper.Map<Article>(createArticleDto);
        var expected = Mapper.Map<ArticleModel>(article);
        expected.AuthorNickName = user.NickName;
        _contextAccessor.Setup(accessor => accessor.HttpContext.User.Claims)
            .Returns(claims);
        _categoryRepository.Setup(categoryRepository => categoryRepository.Query())
            .Returns(category.AsQueryable().BuildMock());
        _userRepository.Setup(userRepository => userRepository.GetByIdAsync(user.Id))
            .ReturnsAsync(user);
        _articleRepository.Setup(repository => repository.AddAsync(It.IsAny<Article>()))
            .ReturnsAsync(article);

        var actual = await _articleService.CreateArticleAsync(createArticleDto);
        actual.Should().BeEquivalentTo(expected, options => options.Excluding(model => model.CreationDate).ExcludingFields());
    }
    [Theory, AutoEntityData]
    public async Task CreateArticleAsync_WhenArticleDtoInvalid_ThrowsException(User user, IEnumerable<ArticleCategory> category)
    {
        var createArticleDto = Fixture.Build<CreateArticleDto>()
            .With(dto => dto.CategoryId, category.First().Id)
            .With(dto => dto.Status, ArticleStatuses.Published)
            .With(dto => dto.Text, string.Empty)
            .Create();
        var claims = new List<Claim>() {new (ClaimTypes.NameIdentifier, user.Id.ToString())};
        var article = Mapper.Map<Article>(createArticleDto);
        var expected = Mapper.Map<ArticleModel>(article);
        expected.AuthorNickName = user.NickName;
        _contextAccessor.Setup(accessor => accessor.HttpContext.User.Claims)
            .Returns(claims);
        _categoryRepository.Setup(categoryRepository => categoryRepository.Query())
            .Returns(category.AsQueryable().BuildMock());
        _userRepository.Setup(userRepository => userRepository.GetByIdAsync(user.Id))
            .ReturnsAsync(user);
        _articleRepository.Setup(repository => repository.AddAsync(It.IsAny<Article>()))
            .ReturnsAsync(article);

        var actual = await _articleService.CreateArticleAsync(createArticleDto);
        
        actual.Should().BeEquivalentTo(expected, options => options.Excluding(model => model.CreationDate).ExcludingFields());
    }

    [Theory, AutoEntityData]
    public async Task UpdateArticleAsync_WhenUpdateArticleDto_IsValid_ReturnsArticle
        (IEnumerable<Article> articles, Article article)
    {
        articles = articles.Append(article);
        var updateArticleDataDto = Fixture.Build<UpdateArticleDataDto>()
            .With(dto => dto.Id, article.Id)
            .Create();
        var expected = Fixture.Build<Article>()
            .With(article => article.Id, article.Id)
            .With(article => article.Name, updateArticleDataDto.Name)
            .With(article => article.Text, updateArticleDataDto.Text)
            .Create();
        
        var claims = new List<Claim>() {new (ClaimTypes.NameIdentifier, article.AuthorId.ToString())};
        _articleRepository.Setup(repository => repository.Query())
            .Returns(articles.AsQueryable().BuildMock());
        var actual = await _articleService.UpdateArticleDataAsync(updateArticleDataDto);
        
    }
}