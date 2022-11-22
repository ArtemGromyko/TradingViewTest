using Quartz;
using Quartz.Impl;
using TradingView.DAL.Quartz.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace TradingView.DAL.Quartz.Schedulers;

public class FinancialsAsReportedScheduler
{
    public static async void Start(IServiceProvider serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<FinancialsAsReportedJob>()
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("FinancialsAsReportedTrigger", "default")
            .WithCronSchedule("0 */5 * ? * *", x => x.InTimeZone(TimeZoneInfo.Utc)) //Updates at 4am and 5am UTC every day
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}
