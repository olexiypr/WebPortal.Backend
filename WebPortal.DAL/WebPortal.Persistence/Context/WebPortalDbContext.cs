using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebPortal.Domain;
using WebPortal.Domain.Complaint;
using WebPortal.Domain.Enums;
using WebPortal.Domain.User;
using WebPortal.Persistence.EntityConfigurations;
using WebPortal.Persistence.SeedData;

namespace WebPortal.Persistence.Context;

public class WebPortalDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserAuth> UserAuths { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Commentary> Commentaries { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }
    public DbSet<ArticleCategory> ArticleCategories { get; set; }
    public DbSet<ArticleComplaint> ArticleReports { get; set; }
    public DbSet<UserComplaint> UserReports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /*var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration["DbConnection"];
        /*var connectionString = "Host=localhost;Username=aloshaprokopenko5;Password=787898;Database=WebPortalDb";#1#
        optionsBuilder.UseNpgsql(connectionString);*/
        optionsBuilder.EnableSensitiveDataLogging();
    }

    public WebPortalDbContext(DbContextOptions<WebPortalDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleComplaintConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new CommentaryConfiguration());
        modelBuilder.ApplyConfiguration(new RecommendationConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new UserAuthConfiguration());
        modelBuilder.ApplyConfiguration(new UserComplaintConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        modelBuilder.Seed();
    }
}