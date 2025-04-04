using System.Text;
using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Interfaces;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;

namespace LicenseService.Domain.Helpers;

public class RsaHelper : IRsaHelper
{
    private readonly string _privateKey;

    public RsaHelper(IConfiguration configuration)
    {
        _privateKey = configuration.GetSection("Certificate:PrivateKey").Value ??
                      throw new NotFoundException("Appsetting", "private key path");
    }

    public string SignCertificate(string text)
    {
        var bytesToSign = Encoding.UTF8.GetBytes(text);
        TextReader reader = File.OpenText(_privateKey);
        var keyPair = (AsymmetricCipherKeyPair) new PemReader(reader).ReadObject();

        var signature = RsaSignWithPrivateKey(keyPair, bytesToSign);
        return Convert.ToBase64String(signature);
    }

    private static byte[] RsaSignWithPrivateKey(AsymmetricCipherKeyPair keyPair,
        byte[] bytesToSign)
    {
        var sha256Digest = new Sha256Digest();
        var theHash = new byte[sha256Digest.GetDigestSize()];
        sha256Digest.BlockUpdate(bytesToSign, 0, bytesToSign.Length);
        sha256Digest.DoFinal(theHash, 0);

        var signer = new PssSigner(new RsaEngine(), new Sha256Digest(),
            sha256Digest.GetDigestSize());
        signer.Init(true, keyPair.Private);
        signer.BlockUpdate(theHash, 0, theHash.Length);
        return signer.GenerateSignature();
    }
}