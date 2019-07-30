using AliyunAcmDemo.Models;
using Colipu.AliyunAcm.Utils.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliyunAcmDemo
{
    /// <summary>
    /// 用单例模式访问配置文件，防止多线程的读取
    /// </summary>
    public class ConfigureAdapterService
    {
        private static ConfigureAdapterService instance;
        private ConfigureAdapterService()
        {
            try
            {
                var defaultJson = File.ReadAllText("D:\\Colipu_ERP\\AliyunAcmDemo\\Colipu.AliyunAcm.Utils\\Configs\\Default.json");
                var defaultModel = JsonHelper.DeserializeObject<AliyunConfig>(defaultJson);
                ConfigureModel = defaultModel;
            }
            catch (Exception ex)
            {
            }
           
        }
        public static ConfigureAdapterService getInstance()
        {
            if (instance == null)
            {
                instance = new ConfigureAdapterService();
            }
            return instance;
        }

        public string JsonFilePath { get; set; }
        public AliyunConfig ConfigureModel { get; set; }

        public string ipAddress { get { return HttpUtitl.GetAcmIpAddress("http://acm.aliyun.com:8080/diamond-server/diamond"); } }

        public string LogFilePath = @"E:\Log.txt";
    }
}
