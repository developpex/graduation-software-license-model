using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

public class RsaEncryption
{
    private static RSACryptoServiceProvider _csp = new(2048);
    private readonly RSAParameters _privateKey;
    private readonly RSAParameters _publicKey;

    public RsaEncryption()
    {
        _privateKey = _csp.ExportParameters(true);
        _publicKey = _csp.ExportParameters(false);
    }

    public string GetPublicKey()
    {
        var sw = new StringWriter();
        var xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(sw, _publicKey);
        return sw.ToString();
    }

    public string Encrypt(string plainText)
    {
        _csp = new RSACryptoServiceProvider();
        _csp.ImportParameters(_publicKey); // use public key om deze te encrypten
        var data = Encoding.Unicode.GetBytes(plainText);
        var cypher = _csp.Encrypt(data, false);
        return Convert.ToBase64String(cypher);
    }

    public string Decrypt(string cypherText)
    {
        var dataBytes = Convert.FromBase64String(cypherText);
        _csp.ImportParameters(_privateKey);
        var plainText = _csp.Decrypt(dataBytes, false);
        return Encoding.Unicode.GetString(plainText);
    }
}
