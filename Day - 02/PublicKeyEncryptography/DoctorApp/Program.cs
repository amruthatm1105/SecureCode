using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
class DoctorApp
{
    static void Main()
    {
        string privateKeyPath = "C:\\Users\\z004wu2m\\source\\repos\\Rsa\\private_key.xml";
        string encryptedDataPath = "C:\\Users\\z004wu2m\\source\\repos\\Rsa\\DoctorApp\\bin\\Debug\\encrypted_patient_data.bin";

        if (!File.Exists(privateKeyPath))
        {
            Console.WriteLine("Error: Private key not found!");
            return;
        }

        byte[] encryptedData = File.ReadAllBytes(encryptedDataPath);
        byte[] decryptedData;

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(File.ReadAllText(privateKeyPath));
            decryptedData = rsa.Decrypt(encryptedData, false);
        }

        string patientData = Encoding.UTF8.GetString(decryptedData);
        Console.WriteLine("Decrypted Patient Data:");
        Console.WriteLine(patientData);
    }
}