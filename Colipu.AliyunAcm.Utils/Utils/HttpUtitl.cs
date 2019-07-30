using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Colipu.AliyunAcm.Utils.Utils
{
    public class HttpUtitl
    {
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, IDictionary<string, string> headerInfo)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;//创建请求对象
            request.Method = "POST";//请求方式
            request.ContentType = "application/x-www-form-urlencoded";//链接类型
            //构造header
            if (!(headerInfo == null || headerInfo.Count == 0))
            {
                foreach (var item in headerInfo)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            //构造查询字符串param
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                bool first = true;
                foreach (string key in parameters.Keys)
                {

                    if (!first)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        first = false;
                    }
                }
                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                //写入请求流
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }


        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpWebResponse CreateGetHttpResponse(string url, Dictionary<string, string> headerInfo)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";//链接类型
            if (!(headerInfo == null || headerInfo.Count == 0))
            {
                foreach (var item in headerInfo)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headerInfo"></param>
        /// <returns></returns>
        public static string GetAcmIpAddress(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            return GetResponseString(request.GetResponse() as HttpWebResponse).Trim();
        }

        /// <summary>
        /// 读取response ,Encoding.GetEncoding("GBK")
        /// </summary>
        /// <param name="webresponse"></param>
        /// <returns></returns>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.GetEncoding("GBK"));
                return reader.ReadToEnd();
            }
        }
    }
}
