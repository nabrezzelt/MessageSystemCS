using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MessageSysDataManagementLib
{
    public class KeyManagement
    {
        public enum KeySize : int
        {
            SIZE_1028 = 1028,
            SIZE_2048 = 2048,
            SIZE_4096 = 4096,
            SIZE_8192 = 8192
        }

        /// <summary>
        /// Generates a RSA-Key Pair.
        /// </summary>
        /// <returns>1: Private Key, 2: Public Key</returns>
        public static Tuple<string, string> CreateKeyPair()
        {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(Convert.ToInt32(KeySize.SIZE_4096), cspParams);

            string publicKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(false));
            string privateKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(true));

            return new Tuple<string, string>(privateKey, publicKey);
        }

        /// <summary>
        /// Encrypt the Message with a given PublicKey.
        /// </summary>
        /// <param name="publicKey">PublicKey</param>
        /// <param name="message">Messagedata</param>
        /// <returns>Encrypted Message as Byte-Array</returns>
        public static byte[] Encrypt(string publicKey, string message)
        {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(publicKey));
            byte[] plainBytes = Encoding.UTF8.GetBytes(message);
            byte[] encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

            return encryptedBytes;
        }

        /// <summary>
        /// Decrypt a Message with given PrivateKey
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="encryptedMessage"></param>
        /// <returns></returns>
        public static string Decrypt(string privateKey, byte[] encryptedMessage)
        {
            CspParameters cspParams = new CspParameters { ProviderType = 1 };
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(privateKey));
            byte[] plainBytes = rsaProvider.Decrypt(encryptedMessage, false);
            string plainText = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);

            return plainText;
        }
    }
}
