using Quartz;
using Quartz.Impl;
using TradingView.DAL.Quartz.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace TradingView.DAL.Quartz.Schedulers;

public class StockDataScheduler
{
    public static async void Start(IServiceProvider serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<StockDataJob>()
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("StockDataTrigger", "default")
            .WithCronSchedule("0 10 21 ? * MON,TUE,WED,THU,FRI *", x => x.InTimeZone(TimeZoneInfo.Utc)) //Updates at 4am and 5am UTC every day
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}
