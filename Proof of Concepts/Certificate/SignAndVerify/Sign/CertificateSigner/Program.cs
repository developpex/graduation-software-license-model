using System;
using System.IO;
using System.Text.Json;

namespace CertificateSigner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string certificateLocation = 
                "C:\\Users\\Patrick\\Desktop\\Afstudeer Opdracht\\Eindproduct\\Certificate\\certificate.txt";

            const string oldCertification = certificateLocation + ".old";
            try
            {
                if (File.Exists(oldCertification))
                {
                    File.Delete(oldCertification);
                }

                if (File.Exists(certificateLocation))
                {
                    File.Move(certificateLocation, oldCertification);
                }

                var certificateSigner = new CertificateSigner();

                using var writer = new StreamWriter(certificateLocation);
                const string textToSign = "text_to_sign_part4";
             
                writer.WriteLine(textToSign);
                writer.WriteLine(string.Empty);
                writer.WriteLine("--- START SIGNATURE ---");
                writer.WriteLine(certificateSigner.RsaSignWithPemPrivateKey(textToSign));
                writer.WriteLine("--- END SIGNATURE ---");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
