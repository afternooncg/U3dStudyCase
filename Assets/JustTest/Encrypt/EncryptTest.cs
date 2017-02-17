using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class EncryptTest : MonoBehaviour {

	// Use this for initialization
    void TestDes()
    {
        string sKey = "L*#)!@&8";
        string pToEncrypt = "1234567891011213141516";


        //Debug.Log("结果:" + Encrypt(pToEncrypt, sKey));

        byte[] b = UTF8Encoding.ASCII.GetBytes(pToEncrypt);
        byte[] b1 = Encoding.ASCII.GetBytes(pToEncrypt);
        byte[] b2 = Encoding.Default.GetBytes(pToEncrypt);
        sbyte f = -1;
        byte e = (byte)f;

        for (int m = 0; m < 8; m++)
        {
            Debug.Log("m=" + m + " " + GetbitValue(f, m));
        }

        DesEncrypt.Encrypt(ref b);

        int c = b.Length / 8;
        int pos = 0;
        for (int i = 0; i < c; i++)
        {
            byte[] buf = new byte[8];
            Buffer.BlockCopy(b, pos, buf, 0, 8);
            DesEncrypt.Encrypt(ref buf);
            Buffer.BlockCopy(buf, 0, b, pos, 8);
            pos += 8;
        }

        Debug.Log("ok");
        //  Debug.Log("CC2加密:" + DesEncrypt.Encrypt(ref b));


        // Debug.Log("解密:" + Decrypt(Encrypt(pToEncrypt, sKey),sKey));


        //0B5705BCF6883C14;
        //24225358812341008659
    }

    private int GetbitValue(object obj, int index)
    {
        int size = Marshal.SizeOf(obj);
        System.IntPtr intPtr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(obj, intPtr, true);
        byte[] byteArr = new byte[size];
        Marshal.Copy(intPtr, byteArr, 0, size);
        int count;
        index = Math.DivRem(index, 8, out count);
        Marshal.FreeHGlobal(intPtr);
        return (byteArr[size - index - 1] >> (8 - count - 1)) & 1;
    }

    public static string Encrypt(string pToEncrypt, string sKey)
    {
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        provider.Mode = CipherMode.ECB;
        provider.Padding = PaddingMode.None;

        byte[] bytes = Encoding.Default.GetBytes(pToEncrypt);
        if (bytes.Length < 8)
        {
            byte[] a = new byte[8];
            Buffer.BlockCopy(bytes, 0, a, 0, bytes.Length);
            bytes = a;
        }
        provider.Key = Encoding.ASCII.GetBytes(sKey);
        provider.IV = Encoding.ASCII.GetBytes(sKey);
        /* 创建一个内存流对象 */
        MemoryStream stream = new MemoryStream();
        /* 创建一个加密流对象 */
        CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
        /* 将要加密的文本写到加密流中 */
        stream2.Write(bytes, 0, bytes.Length);
        /* 更新缓冲 */
        stream2.FlushFinalBlock();
        /* 获取加密过的文本 */
        StringBuilder builder = new StringBuilder();
        StringBuilder builder1 = new StringBuilder();
        byte[] temp = stream.ToArray();
        foreach (byte num in temp)
        {
            builder.AppendFormat("{0:X2}", num);
            builder1.Append(num);
        }
        stream2.Close();
        stream.Close();

        Debug.Log("解密:" + Decrypt(builder.ToString(), sKey));

        return builder.ToString();
        //return Convert.ToBase64String(stream.ToArray());
        //byte[] bytes4 = stream.ToArray();
        //string str=Encoding.Default.GetString(bytes4);
        //return str;
    }
    /// <summary>
    /// DES解密
    /// </summary>
    /// <param name="input">待解密的字符串</param>
    /// <param name="key">解密密钥,要求为8位,和加密密钥相同</param>
    /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
    public static string Decrypt(string DecryptString, string Key)
    {
        try
        {
            //byte[] inputByteArray = Convert.FromBase64String(DecryptString);
            /** 
             **将一个字符串转16进制字节数组而已
             **/
            byte[] inputByteArray = new byte[DecryptString.Length / 2];
            for (int x = 0; x < DecryptString.Length / 2; x++)
            {
                int i = (Convert.ToInt32(DecryptString.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            //byte[] inputByteArray = Encoding.UTF8.GetBytes(DecryptString);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.None;
            des.Key = Encoding.ASCII.GetBytes(Key);
            des.IV = Encoding.ASCII.GetBytes(Key);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            mStream.Close();
            cStream.Close();
            return Encoding.Default.GetString(mStream.ToArray());
        }
        catch
        {
            return "";
        }
    }

    public static string DES3Encrypt(string data, string key)
    {
        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        //DES.GenerateKey();
        //byte[] cKey = DES.Key;
        DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
        DES.Mode = CipherMode.ECB;
        DES.Padding = PaddingMode.None;
        ICryptoTransform DESEncrypt = DES.CreateEncryptor();
        byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(data);
        byte[] result = DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length);
        StringBuilder builder = new StringBuilder();
        foreach (byte num in result)
        {
            builder.AppendFormat("{0:X2}", num);
        }
        return builder.ToString();
        //return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
    }
    public static string DES3Decrypt(string DecryptString, string key)
    {
        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        //DES.GenerateKey();
        DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
        DES.Mode = CipherMode.ECB;
        DES.Padding = PaddingMode.None;
        ICryptoTransform DESDecrypt = DES.CreateDecryptor();
        string result = "";
        try
        {
            //byte[] Buffer = Convert.FromBase64String(data);
            /*字符串转16进制字节数组*/
            byte[] inputByteArray = new byte[DecryptString.Length / 2];
            for (int x = 0; x < DecryptString.Length / 2; x++)
            {
                int i = (Convert.ToInt32(DecryptString.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            //byte[] Byteresult = DESDecrypt.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length);
            //StringBuilder builder = new StringBuilder();
            //foreach (byte num in Byteresult)
            //{
            //    builder.AppendFormat("{0:X2}", num);
            //}
            //return builder.ToString();
            result = ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(inputByteArray, 0, inputByteArray.Length));
        }
        catch (Exception e)
        {
        }
        return result;
    }
}
