using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AliyunAcmDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 监听配置
            Timer timer = new Timer((o) =>
            {
                AccessInterface.BatchUpdateConfigure();
            }, null, 0, 30000);

            //防止主线程退出导致定时器失效
            Console.ReadKey();
            Console.WriteLine("done!");
            #endregion


            #region 方案二，做成windows服务一直占用进程去监听，这样配置响应是即时的

            #endregion

            //AccessInterface.ManualUpdateAllConfigure();
        }
    }
}
