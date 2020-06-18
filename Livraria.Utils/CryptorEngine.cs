using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Livraria.Utils
{
    public interface ICryptorEngine
    {
        string EncryptMD5(string value);
        string Encrypt(string toEncrypt, bool useHashing);
        string Decrypt(string cipherString, bool useHashing);
    }

    public class CryptorEngine : ICryptorEngine
    {
        private readonly IOptions<CryptoSettings> appSettings;

        public CryptorEngine(IOptions<CryptoSettings> appSettings)
        {
            this.appSettings = appSettings;
        }

        public string EncryptMD5(string value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        public string Encrypt(string toEncrypt, bool useHashing)
        {

            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            // Get the key from config file
            //string key = new AppConfiguration().CriptoKey;
            string key = appSettings.Value.CriptoKey;

            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));

                //Always release the resources and flush data of the Cryptographic service provide. Best Practice
                hashmd5.Clear();
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider();

            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;

            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;

            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();

            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Release resources held by TripleDes Encryptor
            tdes.Clear();


            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;

            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //Get your key from config file to open the lock!
            var key = appSettings.Value.CriptoKey;

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                var hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));

                //release any resource held by the MD5CryptoServiceProvider
                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider();

            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;

            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;

            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Release resources held by TripleDes Encryptor                
            tdes.Clear();

            //return the Clear decrypted TEXT
            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
