using AliyunAcmDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliyunAcmDemo
{
    public class UrlGenerate
    {
        /// <summary>
        /// 获取配置url
        /// </summary>
        /// <returns></returns>
        public static string GetConfigureUrl(string ipAddress,string dataId,string group,string tenant)
        {
            return "http://" + ipAddress + ":8080" + "/diamond-server/config.co?dataId=" + dataId + "&group=" + group + "&tenant=" + tenant;
        }

        /// <summary>
        /// 获取监听配置url
        /// </summary>
        /// <returns></returns>
        public static string GetListeningUrl(string ipAddress)
        {
            return "http://"+ ipAddress+":8080"+ "/diamond-server/config.co";
        }

    }
}
