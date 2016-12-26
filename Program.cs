using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Text;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.DependencyInjection;
namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //注册EncodingProvider实现对中文编码的支持
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Func<string,LogLevel,bool>filter=(category,level)=>true;
            //ILoggerFactory loggerFactory=new LoggerFactory();
            //loggerFactory.AddProvider(new ConsoleLoggerProvider(filter,false));
            //loggerFactory.AddProvider(new DebugLoggerProvider(filter));
            //ILogger logger=loggerFactory.CreateLogger(nameof(Program));
            ILogger logger=new ServiceCollection().AddLogging().BuildServiceProvider().GetService<ILoggerFactory>().
            AddConsole().AddDebug().CreateLogger(nameof(Program));
            int eventId=3721;

            logger.LogInformation(eventId,"升级到最新.Net Core版本({version})","1.0.0");
            logger.LogWarning(eventId,"并发量接近上线({maximum})",200);
            logger.LogError(eventId,"数据库连接失败(数据库:{Database},用户名:{User})","TestDb","sa");

            Console.WriteLine("----------2016年12月26日-----------------------");
            ILogger logger2=new ServiceCollection().AddLogging()
            .BuildServiceProvider().GetService<ILoggerFactory>()
            .AddConsole(true)
            .CreateLogger("Ordering");
            //上下文关联输出日志
            using(logger2.BeginScope("订单:{ID}","20160520001"))
            {
                logger2.LogWarning("商品库存不足(商品ID:{0},当前库存:{1},订购数量:{2})","9787121237812",20,50);
                logger2.LogError("商品ID录入错误(商品ID:{0})","9787121235368");
            }
        }
    }
}
