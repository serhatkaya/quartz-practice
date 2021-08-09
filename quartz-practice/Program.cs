using Microsoft.Extensions.Hosting;
using Quartz;
using quartz_practice.Jobs;
using Serilog;

namespace quartz_practice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                      .Enrich.FromLogContext()
                      .WriteTo.Console()
                      .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        // see Quartz.Extensions.DependencyInjection documentation about how to configure different configuration aspects
                        services.AddQuartz(q =>
                        {
                            var jobKey = new JobKey("ShowDateTimeJob");
                            q.AddJob<ShowDateTimeJob>(opts => opts.WithIdentity(jobKey));
                            // your configuration here
                            q.AddTrigger(opts => opts
                               .ForJob(jobKey) // link to the HelloWorldJob
                               .WithIdentity("ShowDateTimeJob-trigger") // give the trigger a unique name
                               .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds
                        })
                        // Quartz.Extensions.Hosting hosting
                        .AddQuartzHostedService(options =>
                        {
                            // when shutting down we want jobs to complete gracefully
                            options.WaitForJobsToComplete = true;
                        }); ;
                    });
    }
}
