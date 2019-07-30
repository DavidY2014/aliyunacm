using AliyunAcmDemo.Models;
using AliyunAcmDemo.Services;
using Colipu.AliyunAcm.Utils.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AliyunAcmDemo
{
    public class AccessInterface
    {
        /// <summary>
        /// 监听批量更新配置
        /// </summary>
        public static void BatchUpdateConfigure()
        {
            var jsonModel = ConfigureAdapterService.getInstance().ConfigureModel;
            var tenant = jsonModel.Tenant;
            var secretKey = jsonModel.SecretKey;
            var accessKey = jsonModel.AccessKey;
            foreach (var item in jsonModel.AliyunItems)
            {
                //必须采用多线程去并发遍历配置，不然会发生阻塞耗时
                if (string.IsNullOrEmpty(item.DataId) || string.IsNullOrEmpty(item.Group) || string.IsNullOrEmpty(item.AppConfigurePath))
                {
                    continue;
                }
                var dataId = item.DataId;
                var group = item.Group;
                var appConfigurePath = item.AppConfigurePath;
                new TaskFactory().StartNew(()=> {
                    AcmService.ListeningRemoteConfigure(dataId, tenant, group, secretKey, accessKey, appConfigurePath);
                });
         
            }
        }


        /// <summary>
        /// 更新单个配置，通过DataId，Group进行唯一指定
        /// </summary>
        public static void UpdateSpecifyConfigure(string DataId,string Group, string AppConfigurePath)
        {
            var jsonModel = ConfigureAdapterService.getInstance().ConfigureModel;
            var tenant = jsonModel.Tenant;
            var secretKey = jsonModel.SecretKey;
            var accessKey = jsonModel.AccessKey;
            var dataId = DataId;
            var group = Group;
            var appConfigurePath = AppConfigurePath;
            AcmService.ListeningRemoteConfigure(dataId, tenant, group, secretKey, accessKey, appConfigurePath);
        }

        /// <summary>
        /// 手动更新全部配置
        /// </summary>
        public static bool ManualUpdateAllConfigure()
        {
            try
            {
                var jsonModel = ConfigureAdapterService.getInstance().ConfigureModel;
                var tenant = jsonModel.Tenant;
                var secretKey = jsonModel.SecretKey;
                var accessKey = jsonModel.AccessKey;
                foreach (var item in jsonModel.AliyunItems)
                {
                    if (string.IsNullOrEmpty(item.DataId) || string.IsNullOrEmpty(item.Group) || string.IsNullOrEmpty(item.AppConfigurePath))
                    {
                        continue;
                    }
                    var dataId = item.DataId;
                    var group = item.Group;
                    var appConfigurePath = item.AppConfigurePath;
                    var ret = AcmService.GetRemoteConfigure(tenant, dataId, group, secretKey, accessKey);
                    FileHelper.WriteJsonToFile(ret, appConfigurePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }
    }

}
