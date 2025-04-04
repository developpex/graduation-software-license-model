using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace PublicKey
{
    public class RsaHelper
    {
        private readonly RSACryptoServiceProvider _publicKey;

        public RsaHelper()
        {
            const string publicPem = @"C:\Users\Patrick\Desktop\Afstudeer Opdracht\Eindproduct\Certificate\publickey.pem";

            _publicKey = GetPublicKeyFromPemFile(publicPem);

        }

        public string Encrypt(string text)
        {
            var encryptedBytes = _publicKey.Encrypt(Encoding.UTF8.GetBytes(text), false);
            return Convert.ToBase64String(encryptedBytes);
        }

        private RSACryptoServiceProvider GetPublicKeyFromPemFile(string filePath)
        {
            using TextReader publicKeyTextReader = new StringReader(File.ReadAllText(filePath));
            var publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

            var rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParam);

            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParams);
            return csp;
        }
    }
}
