using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AliyunAcmDemo
{
    public class AcmAlgorithm
    {
        /// <summary>
        /// 用HmacSHA1做签名
        /// </summary>
        /// <param name="encryptText">需加密密文</param>
        /// <param name="encryptKey">私钥</param>
        /// <returns></returns>
        public static string SpasSignature(string encryptText, string encryptKey)
        {
            return HmacSHA1(encryptText, encryptKey);
        }

        /// <summary>
        /// HmacSHA1 加密算法的C#版,与java版本生成值一致
        /// </summary>
        /// <param name="encryptText"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        private static string HmacSHA1(string encryptText, string encryptKey)
        {
            Encoding encode = Encoding.GetEncoding("UTF-8");
            byte[] byteData = encode.GetBytes(encryptText);
            byte[] byteKey = encode.GetBytes(encryptKey);
            HMACSHA1 hmac = new HMACSHA1(byteKey);
            CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
            cs.Write(byteData, 0, byteData.Length);
            cs.Close();
            return Convert.ToBase64String(hmac.Hash);
        }


        /// <summary>
        /// contentMD5 C#版本的MD5算法
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Md5Content(string content)
        {

            return Md5(content);
        }

        private static string Md5(string value)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(value)) return result;
            using (var md5 = MD5.Create())
            {
                result = GetMd5Hash(md5, value);
            }
            return result;
        }


        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

    }
}
