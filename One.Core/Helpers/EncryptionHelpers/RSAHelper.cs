using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace One.Core.Helpers.EncryptionHelpers
{
    public class RSAHelper
    {
        /// <summary> 生成公钥和私钥对 </summary>
        public static void GeneratePublicAndPrivateKeyInfo()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            using (StreamWriter writer = new StreamWriter("PrivateKey.xml"))  //这个文件要保密...
            {
                string privateKey = rsa.ToXmlString(true);
                writer.WriteLine(privateKey);
            }
            using (StreamWriter writer = new StreamWriter("PublicKey.xml"))
            {
                string publicKey = rsa.ToXmlString(false);
                writer.WriteLine(publicKey);
            }
        }

        /// <summary> 用私钥给数据进行RSA加密 </summary>
        /// <param name="xmlPrivateKey">    私钥(XML格式字符串) </param>
        /// <param name="strEncryptString"> 要加密的数据 </param>
        /// <returns> 加密后的数据 </returns>
        public static string PrivateKeyEncrypt(string xmlPrivateKey, string strEncryptString)
        {
            //加载私钥
            RSACryptoServiceProvider privateRsa = new RSACryptoServiceProvider();
            privateRsa.FromXmlString(xmlPrivateKey);

            //转换密钥
            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetKeyPair(privateRsa);
            IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding"); //使用RSA/ECB/PKCS1Padding格式
                                                                                   //第一个参数为true表示加密，为false表示解密；第二个参数表示密钥

            c.Init(true, keyPair.Private);
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(strEncryptString);

            #region 分段加密

            int bufferSize = (privateRsa.KeySize / 8) - 11;
            byte[] buffer = new byte[bufferSize];
            byte[] outBytes = null;
            //分段加密
            using (MemoryStream input = new MemoryStream(dataToEncrypt))
            using (MemoryStream ouput = new MemoryStream())
            {
                while (true)
                {
                    int readLine = input.Read(buffer, 0, bufferSize);
                    if (readLine <= 0)
                    {
                        break;
                    }
                    byte[] temp = new byte[readLine];
                    Array.Copy(buffer, 0, temp, 0, readLine);
                    byte[] encrypt = c.DoFinal(temp);
                    ouput.Write(encrypt, 0, encrypt.Length);
                }
                outBytes = ouput.ToArray();
            }

            #endregion 分段加密

            //byte[] outBytes = c.DoFinal(DataToEncrypt);//加密
            string strBase64 = Convert.ToBase64String(outBytes);

            return strBase64;
        }

        /// <summary> 用公钥给数据进行RSA解密 </summary>
        /// <param name="xmlPublicKey">     公钥(XML格式字符串) </param>
        /// <param name="strDecryptString"> 要解密数据 </param>
        /// <returns> 解密后的数据 </returns>
        public static string PublicKeyDecrypt(string xmlPublicKey, string strDecryptString)
        {
            //加载公钥
            RSACryptoServiceProvider publicRsa = new RSACryptoServiceProvider();
            publicRsa.FromXmlString(xmlPublicKey);
            RSAParameters rp = publicRsa.ExportParameters(false);

            //转换密钥
            AsymmetricKeyParameter pbk = DotNetUtilities.GetRsaPublicKey(rp);

            IBufferedCipher c = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            //第一个参数为true表示加密，为false表示解密；第二个参数表示密钥
            c.Init(false, pbk);
            byte[] outBytes = null;
            byte[] dataToDecrypt = Convert.FromBase64String(strDecryptString);

            #region 分段解密

            int keySize = publicRsa.KeySize / 8;
            byte[] buffer = new byte[keySize];

            using (MemoryStream input = new MemoryStream(dataToDecrypt))
            using (MemoryStream output = new MemoryStream())
            {
                while (true)
                {
                    int readLine = input.Read(buffer, 0, keySize);
                    if (readLine <= 0)
                    {
                        break;
                    }
                    byte[] temp = new byte[readLine];
                    Array.Copy(buffer, 0, temp, 0, readLine);
                    byte[] decrypt = c.DoFinal(temp);
                    output.Write(decrypt, 0, decrypt.Length);
                }
                outBytes = output.ToArray();
            }

            #endregion 分段解密

            //byte[] outBytes = c.DoFinal(DataToDecrypt);//解密

            string strDec = Encoding.UTF8.GetString(outBytes);
            return strDec;
        }

        /// <summary> 使用公钥加密，分段加密 </summary>
        /// <param name="content">        </param>
        /// <param name="privateKeyPath"> </param>
        /// <returns> </returns>
        public static string EncrytByPublic(string publicKeyPath, string strEncryptString)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(ReadFile(publicKeyPath));
            byte[] originalData = Encoding.UTF8.GetBytes(strEncryptString);
            if (originalData == null || originalData.Length <= 0)
            {
                throw new NotSupportedException();
            }
            if (rsa == null)
            {
                throw new ArgumentNullException();
            }
            byte[] encryContent = null;

            #region 分段加密

            int bufferSize = (rsa.KeySize / 8) - 11;
            byte[] buffer = new byte[bufferSize];
            //分段加密
            using (MemoryStream input = new MemoryStream(originalData))
            using (MemoryStream ouput = new MemoryStream())
            {
                while (true)
                {
                    int readLine = input.Read(buffer, 0, bufferSize);
                    if (readLine <= 0)
                    {
                        break;
                    }
                    byte[] temp = new byte[readLine];
                    Array.Copy(buffer, 0, temp, 0, readLine);
                    byte[] encrypt = rsa.Encrypt(temp, false);
                    ouput.Write(encrypt, 0, encrypt.Length);
                }
                encryContent = ouput.ToArray();
            }

            #endregion 分段加密

            return Convert.ToBase64String(encryContent);
        }

        /// <summary> 使用公钥加密，分段加密 </summary>
        /// <param name="content">        </param>
        /// <param name="privateKeyPath"> </param>
        /// <returns> </returns>
        public static string EncrytByPublic2(string publicKeyContent, string strEncryptString)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyContent);
            byte[] originalData = Encoding.UTF8.GetBytes(strEncryptString);
            if (originalData == null || originalData.Length <= 0)
            {
                throw new NotSupportedException();
            }
            if (rsa == null)
            {
                throw new ArgumentNullException();
            }
            byte[] encryContent = null;

            #region 分段加密

            int bufferSize = (rsa.KeySize / 8) - 11;
            byte[] buffer = new byte[bufferSize];
            //分段加密
            using (MemoryStream input = new MemoryStream(originalData))
            using (MemoryStream ouput = new MemoryStream())
            {
                while (true)
                {
                    int readLine = input.Read(buffer, 0, bufferSize);
                    if (readLine <= 0)
                    {
                        break;
                    }
                    byte[] temp = new byte[readLine];
                    Array.Copy(buffer, 0, temp, 0, readLine);
                    byte[] encrypt = rsa.Encrypt(temp, false);
                    ouput.Write(encrypt, 0, encrypt.Length);
                }
                encryContent = ouput.ToArray();
            }

            #endregion 分段加密

            return Convert.ToBase64String(encryContent);
        }

        /// <summary> 使用公钥加密，分段加密,加密后做特殊处理,预计为网络传输要求 </summary>
        /// <param name="content">        </param>
        /// <param name="privateKeyPath"> </param>
        /// <returns> </returns>
        public static string EncrytByPublicForOppoMes(string publicKeyContent, string strEncryptString)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyContent);
            byte[] originalData = Encoding.UTF8.GetBytes(strEncryptString);
            if (originalData == null || originalData.Length <= 0)
            {
                throw new NotSupportedException();
            }
            if (rsa == null)
            {
                throw new ArgumentNullException();
            }
            byte[] encryContent = null;

            #region 分段加密

            int bufferSize = (rsa.KeySize / 8) - 11;
            byte[] buffer = new byte[bufferSize];
            //分段加密
            using (MemoryStream input = new MemoryStream(originalData))
            using (MemoryStream ouput = new MemoryStream())
            {
                while (true)
                {
                    int readLine = input.Read(buffer, 0, bufferSize);
                    if (readLine <= 0)
                    {
                        break;
                    }
                    byte[] temp = new byte[readLine];
                    Array.Copy(buffer, 0, temp, 0, readLine);
                    byte[] encrypt = rsa.Encrypt(temp, false);
                    ouput.Write(encrypt, 0, encrypt.Length);
                }
                encryContent = ouput.ToArray();
            }

            #endregion 分段加密

            var res1 = Convert.ToBase64String(encryContent);

            byte[] bufferaa = new byte[20480];
            uint count = (uint)Math.Ceiling((float)res1.Length / 64);

            int j = 0;
            for (int i = 0; i < count; i++)
            {
                for (int k = 64 * i; k < 64 * i + 64 && k < res1.Length; k++)
                {
                    bufferaa[j] = (byte)res1[k];
                    j++;
                }

                bufferaa[j] = 10;
                j++;
            }

            byte[] bufferbb = new byte[j];
            Array.Copy(bufferaa, bufferbb, j);
            return Encoding.UTF8.GetString(bufferbb);
        }

        /// <summary> 通过私钥解密，分段解密 </summary>
        /// <param name="content">        </param>
        /// <param name="privateKeyPath"> </param>
        /// <returns> </returns>
        public static string DecryptByPrivate(string privateKeyPath, string strDecryptString)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(ReadFile(privateKeyPath));
            byte[] encryptData = Convert.FromBase64String(strDecryptString);
            //byte[] dencryContent = rsa.Decrypt(encryptData, false);
            byte[] dencryContent = null;

            #region 分段解密

            if (encryptData == null || encryptData.Length <= 0)
            {
                throw new NotSupportedException();
            }

            int keySize = rsa.KeySize / 8;
            byte[] buffer = new byte[keySize];

            using (MemoryStream input = new MemoryStream(encryptData))
            using (MemoryStream output = new MemoryStream())
            {
                while (true)
                {
                    int readLine = input.Read(buffer, 0, keySize);
                    if (readLine <= 0)
                    {
                        break;
                    }
                    byte[] temp = new byte[readLine];
                    Array.Copy(buffer, 0, temp, 0, readLine);
                    byte[] decrypt = rsa.Decrypt(temp, false);
                    output.Write(decrypt, 0, decrypt.Length);
                }
                dencryContent = output.ToArray();
            }

            #endregion 分段解密

            return Encoding.UTF8.GetString(dencryContent);
        }

        /// <summary> 读取文件 </summary>
        /// <param name="filePath"> </param>
        /// <returns> </returns>
        public static string ReadFile(string filePath)
        {
            string content = "";
            if (File.Exists(filePath))
            {
                content = File.ReadAllText(filePath);
                byte[] mybyte = Encoding.UTF8.GetBytes(content);
                content = Encoding.UTF8.GetString(mybyte);
            }
            return content;
        }

        /// <summary> RSA公钥格式转换，java-&gt;.net </summary>
        /// <param name="publicKey"> java生成的公钥 </param>
        /// <returns> </returns>
        public static string RSAPublicKeyJava2DotNet(string publicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
            Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
            Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary> RSA私钥格式转换，java-&gt;.net </summary>
        /// <param name="privateKey"> java生成的RSA私钥 </param>
        /// <returns> </returns>
        public static string RSAPrivateKeyJava2DotNet(string privateKey)
        {
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
            Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }
    }
}