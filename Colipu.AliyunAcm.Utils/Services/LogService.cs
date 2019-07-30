using AliyunAcmDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliyunAcmDemo.Services
{
    public class LogService
    {
        private static string logPath = string.Empty;
        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    logPath = AppDomain.CurrentDomain.BaseDirectory;
                }
                return logPath;
            }
            set { logPath = value; }
        }

        private static string logFielPrefix = string.Empty;
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public static string LogFielPrefix
        {
            get { return logFielPrefix; }
            set { logFielPrefix = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string logFile, string msg)
        {
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(
                       LogPath + LogFielPrefix + logFile + " " +
                       DateTime.Now.ToString("yyyyMMdd") + ".Log"
                       ))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + msg);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(LogFile logFile, string msg)
        {
            WriteLog(logFile.ToString(), msg);
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        Trace,
        Warning,
        Error,
        SQL
    }



}
