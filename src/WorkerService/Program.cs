using Quartz;
using Exceptionless;
using WT.Trigger.Application;
using WT.Trigger.WorkerService;
using WT.Trigger.Infrastructure;
using WT.Trigger.Application.Job;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builer = Host.CreateDefaultBuilder(args);
builer.ConfigureLogging(build => build.AddExceptionless());
builer.ConfigureServices((hostContext, services) => {
    services.AddExceptionless();
    services.AddInfrastructure(hostContext.Configuration);
    services.AddApplication();
    services.AddHostedService<Worker>();
    services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();
        //每两个小时执行一次
        q.ScheduleJob<AsyncTestJob>(trigger => trigger.WithCronSchedule("0 0 0/2 * * ?"));
    });
    services.AddQuartzHostedService(x => x.WaitForJobsToComplete = true);
});
builer.UseExceptionless();
var app = builer.Build();

app.Run();

