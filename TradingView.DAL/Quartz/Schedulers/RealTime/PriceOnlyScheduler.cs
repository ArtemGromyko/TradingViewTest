using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using TradingView.DAL.Quartz.Jobs.RealTime;

namespace TradingView.DAL.Quartz.Schedulers.RealTime;

public static class PriceOnlyScheduler
{
    public static async void Start(IServiceProvider serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
        await scheduler.Start();

        IJobDetail jobDetail = JobBuilder.Create<PriceOnlyJob>().Build();
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("PriceOnlyTrigger", "RealTime")
            .StartNow()
            .WithCronSchedule("0 0 9-0 ? * MON,TUE,WED,THU,FRI *", x => x.InTimeZone(TimeZoneInfo.Utc)) //4:30am-8pm ET Mon-Fri
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}
