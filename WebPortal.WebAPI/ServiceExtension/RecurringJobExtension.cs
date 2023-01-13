using Hangfire;
using Services.Interfaces;

namespace WebPortal.WebAPI.ServiceExtension;

public static class RecurringJobExtension
{
    public static void AddArticleViewerCleaner(this IServiceProvider serviceProvider, IRecurringJobManager recurringJobManager)
    {
        recurringJobManager.AddOrUpdate(
            "ClearArticleViewsPerDay",
            () => serviceProvider.GetService<IArticleService>()!.ClearViewsPerDayAsync(),
            Cron.Daily(0,0));
        recurringJobManager.AddOrUpdate(
            "ClearArticleViewsPerWeek",
            () => serviceProvider.GetService<IArticleService>()!.ClearViewsPerWeekAsync(),
            Cron.Weekly(DayOfWeek.Monday,0));
        recurringJobManager.AddOrUpdate(
            "ClearArticleViewsPerMonth",
            () => serviceProvider.GetService<IArticleService>()!.ClearViewsPerMonthAsync(),
            Cron.Monthly(1,0));
    }
}