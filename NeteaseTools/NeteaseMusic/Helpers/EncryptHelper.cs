using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NeteaseMusic.Helpers
{
    public static class EncryptHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearText"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns>Base64字符串</returns>
        /// https://www.zhanghuanglong.com/detail/csharp-version-of-netease-cloud-music-api-analysis-(with-source-code)
        public static string AesEncrypt(string clearText, string key, string iv)
        {
            try
            {
                if (string.IsNullOrEmpty(clearText)) return null;
                Byte[] toEncryptArray = Encoding.UTF8.GetBytes(clearText);
                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    IV = Encoding.UTF8.GetBytes(iv),
                    Mode = CipherMode.CBC
                };
                ICryptoTransform cTransform = rm.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch /*(Exception ex)*/
            {
                return "";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns>16进制字符串</returns>
        public static string MD5Encrypt(string clearText)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(clearText);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                
                return hashBytes.ToHexString();

            }
        }

        //https://stackoverflow.com/questions/1552330/c-sharp-rsa-with-no-padding
        /// <summary>
        /// 通过公钥进行RSA_NO_PADDING加密
        /// </summary>
        /// <param name="clearText"></param>
        /// <param name="publicKey">"-----BEGIN PUBLIC KEY-----\n"+内容+"\n-----END PUBLIC KEY-----"</param>
        /// <returns>16进制字符串</returns>
        public static string RsaEncryptWithPublic(string clearText, string publicKey)
        {
            //var bytesToEncrypt = Encoding.ASCII.GetBytes(clearText);
            var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new RsaEngine(); // new Pkcs1Encoding (new RsaEngine());
            //using (var txtreader = new StringReader("-----BEGIN PUBLIC KEY-----\n" + publicKey + "\n-----END PUBLIC KEY-----"))
            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();
                encryptEngine.Init(true, keyParameter);
            }

            //var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length).ToHexString();
        }
        #region 加密信息
        //static string i = "Rqcu3wnN3sWlLzWf";
        //exponent Strstatic string e = "010001";
        //static string b = "";
        //modulusStr static string f = "00e0b509f6259df8642dbc35662901477df22677ec152b5ff68ace615bb7b725152b3ab17a876aea8a5aa76d2e417629ec4ee341f56135fccf695280104e0312ecbda92557c93870114af6c9d05c4f7f0c3685b7a46bee255932575cce10b424d813cfe4875d3e82047b97ddef52741d546b8e289dc6935b3ece0462db0a22b8e7";
        #endregion
        //https://blog.csdn.net/weixin_44530979/article/details/87925950
        /// <summary>
        /// 通过exponent和modulus进行RSA_NO_PADDING加密
        /// </summary>
        /// <param name="clearText"></param>
        /// <param name="exponentStr"></param>
        /// <param name="modulusStr"></param>
        /// <returns></returns>
        public static string RsaEncryptWithEM(string clearText, string exponentStr, string modulusStr)
        {
            var exponent = BigInteger.Parse(exponentStr, NumberStyles.HexNumber);
            var modulus = BigInteger.Parse(modulusStr, NumberStyles.HexNumber);
            var inputText = BigInteger.Parse(Encoding.UTF8.GetBytes(clearText).ToHexString(), NumberStyles.HexNumber);
            var calRes = BigInteger.ModPow(inputText, exponent, modulus);
            var padRes = calRes.ToString("X2").PadLeft(256, '0');
            return padRes.Substring(padRes.Length - 256);
        }

        #region Rsa encrypt try 

        //public static System.Numerics.BigInteger Encrypt(string message)
        //{
        //    BigInteger t =  new BigInteger(Encoding.UTF8.GetBytes(message));
        //    BigInteger e1 =  new BigInteger(Encoding.UTF8.GetBytes(e));
        //    BigInteger m =  new BigInteger(Encoding.UTF8.GetBytes(f));
        //    return System.Numerics.BigInteger.ModPow(m, e1, m);
        //}

        //public static System.Numerics.BigInteger Decrypt(System.Numerics.BigInteger encryptedMessage)
        //{
        //    return System.Numerics.BigInteger.ModPow(encryptedMessage, D, N);
        //}
        //public static string RsaEncrypt()
        //{
        //    //RSA existingKey = RsaProvider.FromPem(publicKey);
        //    // rsaParams = existingKey.ExportParameters(false);

        //    //BigInteger n = PrepareBigInteger(rsaParams.Modulus);
        //    //BigInteger e = PrepareBigInteger(rsaParams.Exponent);
        //    BigInteger n = PrepareBigInteger(Encoding.UTF8.GetBytes(f));
        //    BigInteger e = PrepareBigInteger(Encoding.UTF8.GetBytes(b));
        //    BigInteger sig = PrepareBigInteger(Encoding.UTF8.GetBytes(i)); //i
        //    BigInteger paddedMsgVal = BigInteger.ModPow(sig, e, n);

        //    byte[] paddedMsg = paddedMsgVal.ToArray();

        //    if (paddedMsg[paddedMsg.Length - 1] == 0)
        //    {
        //        Array.Resize(ref paddedMsg, paddedMsg.Length - 1);
        //    }

        //    Array.Reverse(paddedMsg);
        //    // paddedMsg is now ready.

        //}
        //private static BigInteger PrepareBigInteger(byte[] unsignedBigEndian)
        //{
        //    // Leave an extra 0x00 byte so that the sign bit is clear
        //    byte[] tmp = new byte[unsignedBigEndian.Length + 1];
        //    Buffer.BlockCopy(unsignedBigEndian, 0, tmp, 1, unsignedBigInteger.Length);
        //    Array.Reverse(tmp);
        //    return new BigInteger(tmp);
        //}


        //public static string RsaEncrypt(string input, string publicKey)
        //{

        //    var inputBytes = Encoding.UTF8.GetBytes(input);

        //    using (var rsa = new RSACryptoServiceProvider(1024))
        //    {
        //        try
        //        {
        //            // client encrypting data with public key issued by server                    
        //            rsa.FromXmlString(publicKey.ToString());

        //            var encryptedData = rsa.Encrypt(inputBytes, true);

        //            var base64Encrypted = Convert.ToBase64String(encryptedData);

        //            return base64Encrypted;
        //        }
        //        finally
        //        {
        //            rsa.PersistKeyInCsp = false;
        //        }
        //    }
        //}
        #endregion
    }
}
