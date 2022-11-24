using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using TradingView.DAL.Quartz.Jobs.RealTime;

namespace TradingView.DAL.Quartz.Schedulers.RealTime;

public static class VolumeByVenueScheduler
{
    public static async void Start(IServiceProvider serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
        await scheduler.Start();

        IJobDetail jobDetail = JobBuilder.Create<VolumeByVenueJob>().Build();
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("VolumeByVenueTrigger", "RealTime")
            .StartNow()
            .WithCronSchedule("0 0 14-20 ? * MON,TUE,WED,THU,FRI *", x => x.InTimeZone(TimeZoneInfo.Utc)) //Updates at 8am, 9am UTC daily
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}
