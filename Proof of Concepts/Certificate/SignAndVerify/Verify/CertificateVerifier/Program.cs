using System;
using System.IO;

namespace CertificateVerifier
{
    internal class Program
    {
        static void Main(string[] args)
        {

            const string certificationLocation =
                "C:\\Users\\Patrick\\Desktop\\Afstudeer Opdracht\\Eindproduct\\Certificate\\certificate.txt";

            //using var streamReader = new StreamReader(fileName);
            var lines = File.ReadAllLines(certificationLocation);
            
            var textToVerify = lines[0];
            var signatureToVerify = lines[3];

            Console.WriteLine(textToVerify);
            Console.WriteLine(signatureToVerify);
            
            var certificateVerifier = new CertificateVerifier();
            Console.WriteLine(certificateVerifier.VerifySignature(textToVerify, signatureToVerify));


        }
    }
}
