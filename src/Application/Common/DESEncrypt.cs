using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WT.DirectLogistics.Application.Common
{
    public static class DesEncrypt
    {

        public static string GetPassWordHash(string input)
        {
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new();
            foreach (var item in hashBytes)
            {
                sb.Append(item.ToString("X2"));
            }
            return sb.ToString().Substring(8, 16);
        }

        #region ========密码加密========

        /// <summary>
        /// 密码md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string HexToMd5(string str, int mode)
        {
            string strHex = "";
            StringBuilder md5_string = new StringBuilder();
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            byte[] result = md.ComputeHash(Encoding.Default.GetBytes(str));
            for (int i = 0; i < 16; i++)
            {
                md5_string.Append(string.Format( "{0:x2}", result[i]));
            }
            return (mode == 16) ? md5_string.ToString().Substring(8, 16) : md5_string.ToString();
        }

        #endregion 

        #region AES加密解密
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="sourceStr">密文</param>
        /// <param name="cryptoKey">密钥（物通网密钥：chinawutong!@#）</param>
        /// <returns>解密成功返回【解密结果】，解密失败返回【401状态码】</returns>
        public static string AesDecryptBase64(string sourceStr, string cryptoKey = "chinawutong!@#")
        {
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
                byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
                aes.Key = key;
                aes.IV = iv;
                byte[] dataByteArray = Convert.FromBase64String(sourceStr);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cryptoKey"></param>
        /// <returns></returns>
        public static string AesEncryptBase64(string input, string cryptoKey = "chinawutong!@#")
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
            byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
            aes.Key = key;
            aes.IV = iv;
            byte[] dataByteArray = Encoding.UTF8.GetBytes(input);
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                string encrypt = Convert.ToBase64String(ms.ToArray());
                return encrypt;
            }
        }
        #endregion

        #region ========加密========

        public static string DesMD5(string key)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "").ToUpper();
            }
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "chinawutong");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            AesCryptoServiceProvider des = new AesCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(DesMD5(sKey).Substring(0, 32));
            des.IV = ASCIIEncoding.ASCII.GetBytes(DesMD5(sKey).Substring(0, 16));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "chinawutong");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            AesCryptoServiceProvider des = new AesCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(DesMD5(sKey).Substring(0, 32));
            des.IV = ASCIIEncoding.ASCII.GetBytes(DesMD5(sKey).Substring(0, 16));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }
}
