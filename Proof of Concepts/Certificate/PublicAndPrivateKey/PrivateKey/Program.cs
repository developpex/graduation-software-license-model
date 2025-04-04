using System;
using System.Reflection.Metadata.Ecma335;

namespace PrivateKey
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rsaHelper = new RsaHelper();
            const string fileName =
                @"C:\Users\Patrick\Desktop\Afstudeer Opdracht\Eindproduct\Certificate\certificate.txt";

            var text = System.IO.File.ReadAllText(fileName);
            Console.WriteLine($"Encrypted: {text}\n");
            var decryptedText = rsaHelper.Decrypt(text);
            Console.WriteLine($"Decrypted Json: {decryptedText}\n");
        }
    }
}
