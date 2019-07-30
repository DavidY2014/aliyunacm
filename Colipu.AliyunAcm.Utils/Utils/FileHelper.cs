using AliyunAcmDemo.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colipu.AliyunAcm.Utils.Utils
{
    public class FileHelper
    {
        static object _locker = new object();
        /// <summary>
        /// json写入文件
        /// </summary>
        public static void WriteJsonToFile(string content, string configFilePath)
        {
            try
            {
                lock (_locker)
                {
                    System.IO.File.WriteAllText(configFilePath, content, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                LogService.WriteLog(LogFile.Error, DateTime.Now + ex.ToString());
            }

        }

        /// <summary>
        /// 从文件读取json
        /// </summary>
        /// <returns></returns>
        public static string ReadJsonFromFile(string configFilePath)
        {
            try
            {
                lock (_locker)
                {
                    if (!System.IO.File.Exists(configFilePath))
                    {
                        return null;
                    }
                    return File.ReadAllText(configFilePath);
                }
            }
            catch (Exception ex)
            {
                LogService.WriteLog(LogFile.Error, DateTime.Now + ex.ToString());
                return null;
            }

        }
    }
}
