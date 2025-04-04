using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace PrivateKey
{
    public class RsaHelper
    {
        private readonly RSACryptoServiceProvider _privateKey;

        public RsaHelper()
        {
            const string privatePem = @"C:\Users\Patrick\Desktop\Afstudeer Opdracht\Eindproduct\Certificate\privatekey.pem";

            _privateKey = GetPrivateKeyFromPemFile(privatePem);

        }

        public string Decrypt(string encrypted)
        {
            var decryptedBytes = _privateKey.Decrypt(Convert.FromBase64String(encrypted), false);
            return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
        }

        private RSACryptoServiceProvider GetPrivateKeyFromPemFile(string filePath)
        {
            var csp = new RSACryptoServiceProvider();
            using TextReader privateKeyTextReader = new StringReader(File.ReadAllText(filePath));
            var readKeyPair = (AsymmetricCipherKeyPair)new PemReader(privateKeyTextReader).ReadObject();

            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)readKeyPair.Private);
            csp.ImportParameters(rsaParams);
            return csp;
        }
    }
}
