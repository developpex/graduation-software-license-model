using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;

namespace CertificateSigner
{
    public class CertificateSigner
    {
        private string PrivatePemPath { get; set; }

        public CertificateSigner()
        {
            PrivatePemPath =
                "C:\\Users\\Patrick\\Desktop\\Afstudeer Opdracht\\Eindproduct\\Certificate\\privatekey.pem";
        }

        public string RsaSignWithPemPrivateKey(string text)
        {
            var bytesToSign = Encoding.UTF8.GetBytes(text);
            TextReader reader = File.OpenText(PrivatePemPath);
            var keyPair = (AsymmetricCipherKeyPair) new PemReader(reader).ReadObject();

            var signature = RsaSignWithPrivateKey(keyPair, bytesToSign);
            var result = Convert.ToBase64String(signature);

            return result;
        }

        private static byte[] RsaSignWithPrivateKey(AsymmetricCipherKeyPair keyPair,
            byte[] bytesToSign)
        {
            // compute the SHA 256 hash from the bytes to sign received
            var sha256Digest = new Sha256Digest();
            var theHash = new byte[sha256Digest.GetDigestSize()];
            sha256Digest.BlockUpdate(bytesToSign, 0, bytesToSign.Length);
            sha256Digest.DoFinal(theHash, 0);

            var signer = new PssSigner(new RsaEngine(), new Sha256Digest(),
                sha256Digest.GetDigestSize());
            signer.Init(true, keyPair.Private);
            signer.BlockUpdate(theHash, 0, theHash.Length);
            var signature = signer.GenerateSignature();

            return signature;
        }
    }
}
