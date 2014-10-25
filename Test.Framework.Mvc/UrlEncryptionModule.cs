using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Test.Framework.Extensions;

namespace Test.Framework.Mvc
{
    public class UrlEncryptionModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication httpApp)
        {
            httpApp.BeginRequest += new EventHandler(this.OnBeginRequest);
        }

        #endregion

        #region Private Methods

        void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext currentContext = HttpContext.Current;
            HttpRequest currentRequest = currentContext.Request;

            if (currentRequest.RawUrl.Contains("?"))
            {
                string query = currentRequest.Url.Query.Replace("?", "");
                string path = currentRequest.Url.AbsolutePath.Substring(1);

                if (query.StartsWith("enc=", StringComparison.OrdinalIgnoreCase))
                {
                    string decryptedQuery = Decrypt(query.Replace("enc=", ""));
                    if (decryptedQuery.IsNotNullOrEmpty())
                    {
                        currentContext.RewritePath(string.Format("/{0}", path), string.Empty, decryptedQuery);
                    }
                }

                if (query.StartsWith("anc=", StringComparison.OrdinalIgnoreCase))
                {
                    string decryptedQuery = DecryptAndroid(query.Replace("anc=", ""));
                    if (decryptedQuery.IsNotNullOrEmpty())
                    {
                        currentContext.RewritePath(string.Format("/{0}", path), string.Empty, decryptedQuery);
                    }
                }
            }
        }

        private readonly string passPhrase = "@xx3$sMobi1eApi1";

        private string Encrypt(string plainSourceStringToEncrypt)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider cryptoServiceProvider = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                ICryptoTransform cryptoTransform = cryptoServiceProvider.CreateEncryptor();

                //Set up stream to contain the encryption
                MemoryStream memoryStream = new MemoryStream();

                //Perform the encrpytion, storing output into the stream
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                cryptoStream.Write(sourceBytes, 0, sourceBytes.Length);
                cryptoStream.FlushFinalBlock();

                //sourceBytes are now encrypted as an array of secure bytes
                byte[] encryptedBytes = memoryStream.ToArray();

                //return the encrypted bytes as a BASE64 encoded string
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        private string Decrypt(string base64StringToDecrypt)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider cryptoServiceProvider = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                byte[] rawBytes = Convert.FromBase64String(base64StringToDecrypt);
                ICryptoTransform cryptoTransform = cryptoServiceProvider.CreateDecryptor();

                //RawBytes now contains original byte array, still in Encrypted state

                //Decrypt into stream
                MemoryStream memoryStream = new MemoryStream(rawBytes, 0, rawBytes.Length);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
                //csD now contains original byte array, fully decrypted

                //return the content of msD as a regular string
                return (new StreamReader(cryptoStream)).ReadToEnd();
            }
        }

        private AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] RealKey = GetKey(key, result);
            result.Key = RealKey;
            return result;
        }

        private byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm algorithm)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < algorithm.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            return kList.ToArray();
        }

        public string DecryptAndroid(string base64StringToDecrypt)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;

            //string decodedUrl = HttpUtility.UrlDecode(base64StringToDecrypt);
            byte[] encryptedData = Convert.FromBase64String(base64StringToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(passPhrase);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return encoding.GetString(plainText);
        }
        #endregion
    }
}
