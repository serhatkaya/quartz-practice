using Quartz;
using System;
using System.Threading.Tasks;

namespace quartz_practice.Jobs
{
    public class ShowDateTimeJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"DateTime: {DateTime.Now.ToString("HH:mm:ss")}");
            return Task.CompletedTask;
        }
    }
}
