var rsa = new RsaEncryption();
string cypher = string.Empty;

Console.WriteLine($"Public key {rsa.GetPublicKey()} \n");

Console.WriteLine("enter your text to encrypt");
var text = Console.ReadLine();
if (!string.IsNullOrEmpty(text))
{
    cypher = rsa.Encrypt(text);
    Console.WriteLine($"Encryped Text: {cypher} \n");
}

Console.WriteLine("Press any key to decrypt text");
Console.ReadLine();
var plainText = rsa.Decrypt(cypher);
Console.WriteLine(plainText);

Console.ReadLine();
