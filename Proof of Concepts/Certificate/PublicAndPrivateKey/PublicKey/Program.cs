using System;
using System.IO;

namespace PublicKey
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string textToEncrypt = "Dit wil ik encrypten met mijn public key";
            
            var rsaHelper = new RsaHelper();

            const string fileName = @"C:\Users\Patrick\Desktop\Afstudeer Opdracht\Proof of Concepts\Certificate\PublicAndPrivateKey\EncryptedFile\encrypted.txt";
            try
            {
                using var writer = new StreamWriter(fileName);
                writer.Write(rsaHelper.Encrypt(textToEncrypt));
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message);
            }
        }
    }
}
