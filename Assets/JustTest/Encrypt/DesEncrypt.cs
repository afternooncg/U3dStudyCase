using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

    //对应 C++项目服务端的特定Des加解密工具
    public class DesEncrypt
    {
        public static System.Security.Cryptography.DES mydes = new DESCryptoServiceProvider();
        private static string m_key = "L*#)!@&8";
        private static string m_iv = "";
        private static bool m_isInited = false;

        private static void inits()
        {
            mydes.Mode = CipherMode.ECB;
            mydes.Padding = PaddingMode.None;
            mydes.Key = ASCIIEncoding.ASCII.GetBytes(m_key);
            mydes.IV = ASCIIEncoding.ASCII.GetBytes(m_key);

            if (m_key == "")
                UnityEngine.Debug.Log("没有正确的Key");

            m_isInited = true;
        }


        #region 解密字符串 返回解密后的字符串 string Decrypt(string src, string key, string iv)
        public static string Decrypt(string src, string key, string iv)
        {
            if (!m_isInited)
                inits();

            string result = "";
            try
            {
                byte[] btFile = Convert.FromBase64String(src);
                MemoryStream mStream = new MemoryStream();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream encStream = new CryptoStream(mStream, encrypto, CryptoStreamMode.Write);
                encStream.Write(btFile, 0, btFile.Length);
                encStream.FlushFinalBlock();
                result = Encoding.Default.GetString(mStream.ToArray());
                encStream.Close();
                mStream.Close();
            }
            catch (Exception ex)
            {
                return "解密失败";
            }
            return result;
        }
        #endregion



        #region 加密字符串 返回加密后的
        public static string Encrypt(ref byte[] msgBytes)
        {
            if (!m_isInited)
                inits();

            string result = "";
            try
            {
                MemoryStream mStream = new MemoryStream();
                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream encStream = new CryptoStream(mStream, encrypto, CryptoStreamMode.Write);
                encStream.Write(msgBytes, 0, msgBytes.Length);
                encStream.FlushFinalBlock();
                result = Convert.ToBase64String(mStream.ToArray());
                Buffer.BlockCopy(mStream.ToArray(), 0, msgBytes, 0, 8);
                encStream.Close();
                mStream.Close();
            }
            catch (Exception ex)
            {
                return "";
            }
            return result;
        }
        #endregion






    }
