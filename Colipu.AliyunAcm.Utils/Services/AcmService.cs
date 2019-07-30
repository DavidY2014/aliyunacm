using AliyunAcmDemo.Models;
using AliyunAcmDemo.Services;
using Colipu.AliyunAcm.Utils.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AliyunAcmDemo
{
    public class AcmService
    {
   
        /// <summary>
        /// 监听配置
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="headerInfo"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string ListeningConfigure(IDictionary<string,string> parameters ,Dictionary<string,string> headerInfo,string url)
        {
            var resp = HttpUtitl.CreatePostHttpResponse(url, parameters,headerInfo);
            return HttpUtitl.GetResponseString(resp);
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        private static string GetConfigure(Dictionary<string,string> headerInfo,string url)
        {
            var resp = HttpUtitl.CreateGetHttpResponse(url,headerInfo);
            var content = HttpUtitl.GetResponseString(resp);
            return content;

        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteConfigure(string tenant, string dataId, string group, string secretKey, string accessKey)
        {
            try
            {
                #region 配置信息
                long timeStamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                var spasSignature = AcmAlgorithm.SpasSignature(tenant + "+" + group + "+" + timeStamp, secretKey);
                var headerInfo = new Dictionary<string, string>();
                headerInfo.Add("Spas-AccessKey", accessKey);
                headerInfo.Add("timeStamp", timeStamp.ToString());
                headerInfo.Add("Spas-Signature", spasSignature);
                //headerInfo.Add("Spas-SecurityToken",null);//非必须
                #endregion
                var url = UrlGenerate.GetConfigureUrl(ConfigureAdapterService.getInstance().ipAddress, dataId, group, tenant);
                var jsonContent = GetConfigure(headerInfo, url);
                return jsonContent;
            }
            catch (Exception ex)
            {
                LogService.WriteLog(LogFile.Error, DateTime.Now + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// 监听配置 ,需要重新生成一套时间戳和签名,配置文件来源于本地文件，并放到缓存，其中probeModifyRequest 为dataId^2group^2contentMD5^2tenant^1
        /// </summary>
        /// <returns></returns>
        public static void ListeningRemoteConfigure(string dataId, string tenant, string group, string secretKey, string accessKey, string appConfigurePath)
        {
            try
            {
                var localContent = FileHelper.ReadJsonFromFile(appConfigurePath);
                if (localContent == null)
                {
                    var ret = GetRemoteConfigure(tenant, dataId, group, secretKey, accessKey);
                    FileHelper.WriteJsonToFile(ret, appConfigurePath);
                }
                var contentMd5 = AcmAlgorithm.Md5Content(localContent);
                var probeModifyRequest = dataId + char.ToString((char)2) + group + char.ToString((char)2) + contentMd5 + char.ToString((char)2) + tenant + char.ToString((char)1);
                long timeStamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                var spasSignature = AcmAlgorithm.SpasSignature(tenant + "+" + group + "+" + timeStamp, secretKey);
                var param = new Dictionary<string, string>();
                param.Add("Probe-Modify-Request", probeModifyRequest);
                var headerInfo = new Dictionary<string, string>();
                headerInfo.Add("longPullingTimeout", "30000");
                headerInfo.Add("Spas-AccessKey", accessKey);
                headerInfo.Add("timeStamp", timeStamp.ToString());
                headerInfo.Add("Spas-Signature", spasSignature);
                var url = UrlGenerate.GetListeningUrl(ConfigureAdapterService.getInstance().ipAddress);
                var listenRet = ListeningConfigure(param, headerInfo, url);
                LogService.WriteLog(LogFile.Trace, "监听配置" + DateTime.Now + listenRet);

                #region 监听配置信息，如果发生变更，就重新拉取远程的配置文件
                if (listenRet == string.Empty)
                {
                    LogService.WriteLog(LogFile.Trace, DateTime.Now + "配置没有发生变更");
                }
                else
                {
                    LogService.WriteLog(LogFile.Trace, DateTime.Now + "配置发生变更，获取新信息");
                    var ret = GetRemoteConfigure(tenant, dataId, group, secretKey, accessKey);
                    FileHelper.WriteJsonToFile(ret, appConfigurePath);
                    LogService.WriteLog(LogFile.Trace, DateTime.Now + ret);
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogService.WriteLog(LogFile.Error, DateTime.Now + ex.ToString());
            }

        }






    }
}
