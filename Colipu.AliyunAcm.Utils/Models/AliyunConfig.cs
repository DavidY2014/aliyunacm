using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliyunAcmDemo.Models
{
    /// <summary>
    /// 空间名
    /// </summary>
    public class AliyunConfig
    {
        /// <summary>
        /// 空间名
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string SecretKey { get; set; }


        /// <summary>
        /// 接入密钥
        /// </summary>
        public string AccessKey { get; set; }

        public List<AliyunItem> AliyunItems { get; set; }

    }


    /// <summary>
    /// 配置项
    /// </summary>
    public class AliyunItem
    {
        public string DataId { get; set; }
        public string Group { get; set; }

        public string AppConfigurePath { get; set; }
    }

}
