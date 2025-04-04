using System.Text;
using LicenseManager.Domain.Exceptions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;

namespace LicenseManager.Domain.Helpers;

public class RsaHelper : IRsaHelper
{
    private readonly string _publicKey;

    public RsaHelper(IConfiguration configuration)
    {
        _publicKey = configuration.GetSection("Certificate:PublicKey").Value ??
                     throw new NotFoundException("Appsetting", "public key path");
    }

    public string ReadCertificate(string certificate)
    {
        if (certificate == string.Empty)
        {
            throw new ArgumentException($"Certificate {certificate} not found");
        }

        var lines = File.ReadAllLines(certificate);
        var license = lines[0];
        var signatureToVerify = lines[3];

        if (!SignatureIsVerified(license, signatureToVerify))
        {
            throw new UnableToVerifySignatureException(certificate);
        }

        return license;
    }

    private bool SignatureIsVerified(string text, string expectedSignature)
    {
        var bytesToSign = Encoding.UTF8.GetBytes(text);
        var expectedSignatureBytes = Convert.FromBase64String(expectedSignature);

        TextReader reader = File.OpenText(_publicKey);
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

public interface IRsaHelper
{
    string ReadCertificate(string certificate);
}
