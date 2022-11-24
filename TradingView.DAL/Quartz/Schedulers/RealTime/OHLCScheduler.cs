using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using TradingView.DAL.Quartz.Jobs.RealTime;

namespace TradingView.DAL.Quartz.Schedulers.RealTime;

public static class OHLCScheduler
{
    public static async void Start(IServiceProvider serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
        await scheduler.Start();

        IJobDetail jobDetail = JobBuilder.Create<OHLCJob>().Build();
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("OHLCTrigger", "RealTime")
            .StartNow()
            .WithCronSchedule("0 0 14-21 ? * MON,TUE,WED,THU,FRI *", x => x.InTimeZone(TimeZoneInfo.Utc)) //Updates at  9.30am-5pm ET M-F
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}
