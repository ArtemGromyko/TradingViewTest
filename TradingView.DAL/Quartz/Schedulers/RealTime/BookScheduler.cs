using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using TradingView.DAL.Quartz.Jobs.RealTime;

namespace TradingView.DAL.Quartz.Schedulers.RealTime;

public static class BookScheduler
{
    public static async void Start(IServiceProvider serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
        await scheduler.Start();

        IJobDetail jobDetail = JobBuilder.Create<BookJob>().Build();
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("BookTrigger", "RealTime")
            .StartNow()
            .WithCronSchedule("0 0 * ? * MON,TUE,WED,THU,FRI *", x => x.InTimeZone(TimeZoneInfo.Utc)) //Updates every hour
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}
