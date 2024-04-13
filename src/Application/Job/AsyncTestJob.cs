using Quartz;
using System;
using System.Threading.Tasks;

namespace WT.Trigger.Application.Job
{
    public class AsyncTestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("当前程序是定时执行，执行时间："+DateTime.Now);
        }
    }
}
