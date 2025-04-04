using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;

namespace CertificateVerifier
{
    public class CertificateVerifier
    {
        private string PublicPemPath { get; }

        public CertificateVerifier()
        {
            PublicPemPath =
                "C:\\Users\\Patrick\\Desktop\\Afstudeer Opdracht\\Eindproduct\\Certificate\\publickey.pem";
        }

        public bool VerifySignature(string text, string expectedSignature)
        {
            var bytesToSign = Encoding.UTF8.GetBytes(text);
            var expectedSignatureBytes = Convert.FromBase64String(expectedSignature);

            TextReader reader = File.OpenText(PublicPemPath);
            var keyPair = (AsymmetricKeyParameter) new PemReader(reader).ReadObject();

            var sha256Digest = new Sha256Digest();
            var theHash = new byte[sha256Digest.GetDigestSize()];
            sha256Digest.BlockUpdate(bytesToSign, 0, bytesToSign.Length);
            sha256Digest.DoFinal(theHash, 0);

            var signer = new PssSigner(new RsaEngine(), new Sha256Digest(),
                sha256Digest.GetDigestSize());
            signer.Init(false, keyPair);
            signer.BlockUpdate(theHash, 0, theHash.Length);
            return signer.VerifySignature(expectedSignatureBytes);
        }
    }
}
