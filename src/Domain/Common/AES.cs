using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WT.DirectLogistics.Domain.Common
{
    public class AES
    {
        //警告：此程序中，加密密钥和初始向量请勿随意修改，否则将造成已加密数据解密失败！

        private SymmetricAlgorithm mobjCryptoService;
        private string Key;

        /// <summary> 
        /// 对称加密AES的构造函数
        /// </summary> 
        public AES()
        {
            mobjCryptoService = new RijndaelManaged();
            //Key = "gI~k36J%#u%l9p&)bkA@7fj^oe&(N64*@&*%6je5729Y79fnlu9765^&(*a7oi=3";  //密钥2015-12-02
            mobjCryptoService.BlockSize = 128;
            Key = ")bSe572~%$u%l9p^";  //密钥2016-04-18
        }

        /// <summary> 
        /// 获得密钥 
        /// </summary> 
        /// <returns>密钥</returns> 
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary> 
        /// 获得初始向量IV
        /// </summary> 
        /// <returns>初试向量IV</returns> 
        private byte[] GetLegalIV()
        {
            //string sTemp = "HudhKhoH&$Hygw9q5u^&*i93w786TYey938OE092=-)7j2*%hG@o478&*^$$#KHi";  //初始向量2015-12-02
            string sTemp = "7j2*%hG@o4iHudy9";  //初始向量2016-04-18
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="Source">待加密的串</param>
        /// <returns>经过加密的串，失败返回源串</returns>
        public string Encrypto(string Source)
        {
            try
            {
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
                MemoryStream ms = new MemoryStream();
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch
            {
                return Source;
            }
        }

        /// <summary> 
        /// 解密方法 
        /// </summary> 
        /// <param name="Source">待解密的串</param> 
        /// <returns>经过解密的串</returns> 
        public string Decrypto(string Source)
        {
            try
            {
                byte[] bytIn = Convert.FromBase64String(Source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch
            {
                return Source;
            }
        }
    }
}
