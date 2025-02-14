using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

class PatientApp
{
    static void Main()
    {
        string publicKeyPath = "C:\\Users\\z004wu2m\\source\\repos\\Rsa\\public_key.xml";
        string patientDataPath = "C:\\Users\\z004wu2m\\source\\repos\\Rsa\\patient_data.txt";
        string encryptedDataPath = "encrypted_patient_data.bin";

        if (!File.Exists(publicKeyPath))
        {
            Console.WriteLine("Error: Doctor's public key not found!");
            return;
        }

        string patientData = File.ReadAllText(patientDataPath);
        byte[] dataBytes = Encoding.UTF8.GetBytes(patientData);
        byte[] encryptedData;

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(File.ReadAllText(publicKeyPath));
            encryptedData = rsa.Encrypt(dataBytes, false);
        }

        File.WriteAllBytes(encryptedDataPath, encryptedData);
        Console.WriteLine("Patient data encrypted and saved successfully.");
    }
}
