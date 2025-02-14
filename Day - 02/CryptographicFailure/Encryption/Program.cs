using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using BCrypt.Net;

class Program
{
    static string adminFilePath = "admin_data.txt";
    static string patientFilePath = "patient_data.txt";
    static string encryptionKey; // Store encryption key
    static int maxLoginAttempts = 3;

    static void Main()
    {
        // Generate and store a single encryption key (base64 encoded)
        encryptionKey = GenerateEncryptionKey();

        while (true)
        {
            Console.WriteLine("\n1. Register Admin");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RegisterAdmin();
                    break;
                case "2":
                    if (Login())
                    {
                        PatientManagement();
                    }
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    static void RegisterAdmin()
    {
        Console.Write("Enter Admin Username: ");
        string username = Console.ReadLine();

        if (AdminExists(username))
        {
            Console.WriteLine("Admin username already exists.");
            return;
        }

        Console.Write("Enter Admin Passcode: ");
        string passcode = Console.ReadLine();

        string hashedPasscode = BCrypt.Net.BCrypt.HashPassword(passcode, BCrypt.Net.BCrypt.GenerateSalt(15));

        File.AppendAllText(adminFilePath, $"{username}:{hashedPasscode}\n");
        Console.WriteLine("Admin registered successfully.");
    }

    static bool Login()
    {
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();

        if (!AdminExists(username))
        {
            Console.WriteLine("Admin not found.");
            return false;
        }

        int attempts = 0;
        while (attempts < maxLoginAttempts)
        {
            Console.Write("Enter Passcode: ");
            string passcode = Console.ReadLine();

            if (VerifyPassword(username, passcode))
            {
                Console.WriteLine("Login successful!");
                return true;
            }
            else
            {
                attempts++;
                Console.WriteLine($"Incorrect passcode. Attempts left: {maxLoginAttempts - attempts}");
            }
        }

        Console.WriteLine("Too many failed attempts. Access locked.");
        return false;
    }

    static bool AdminExists(string username)
    {
        if (!File.Exists(adminFilePath)) return false;

        foreach (string line in File.ReadAllLines(adminFilePath))
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2 && parts[0] == username)
                return true;
        }
        return false;
    }

    static bool VerifyPassword(string username, string passcode)
    {
        if (!File.Exists(adminFilePath)) return false;

        foreach (string line in File.ReadAllLines(adminFilePath))
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2 && parts[0] == username)
            {
                return BCrypt.Net.BCrypt.Verify(passcode, parts[1]);
            }
        }
        return false;
    }

    static void PatientManagement()
    {
        while (true)
        {
            Console.WriteLine("\n1. Add Patient Record");
            Console.WriteLine("2. View Patient Records");
            Console.WriteLine("3. Logout");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddPatientRecord();
                    break;
                case "2":
                    ViewPatientRecords();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    static void AddPatientRecord()
    {
        Console.Write("Enter Patient Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Age: ");
        string age = Console.ReadLine();

        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        Console.Write("Enter SSN: ");
        string ssn = Console.ReadLine();

        Console.Write("Enter Medical History: ");
        string history = Console.ReadLine();

        string patientData = $"{name},{age},{email},{ssn},{history}";

        // Generate a unique IV for this record
        string base64IV = GenerateIV();

        // Encrypt patient data
        string encryptedData = Encrypt(patientData, encryptionKey, base64IV);

        // Store the encrypted data in the file
        File.AppendAllText(patientFilePath, $"{base64IV}:{encryptedData}\n");

        Console.WriteLine("Patient record added successfully.");
    }

    static void ViewPatientRecords()
    {
        if (!File.Exists(patientFilePath) || new FileInfo(patientFilePath).Length == 0)
        {
            Console.WriteLine("No patient records found.");
            return;
        }

        Console.WriteLine("\nPatient Records:");
        foreach (string line in File.ReadAllLines(patientFilePath))
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2)
            {
                string base64IV = parts[0];
                string encryptedData = parts[1];

                string decryptedData = Decrypt(encryptedData, encryptionKey, base64IV);
                Console.WriteLine($"- {decryptedData}");
            }
        }
    }

    static string Encrypt(string plaintext, string base64Key, string base64IV)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(base64Key);
            aes.IV = Convert.FromBase64String(base64IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plaintext);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    static string Decrypt(string encryptedText, string base64Key, string base64IV)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(base64Key);
            aes.IV = Convert.FromBase64String(base64IV);

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }

    static string GenerateEncryptionKey()
    {
        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }
    }

    static string GenerateIV()
    {
        using (Aes aes = Aes.Create())
        {
            aes.GenerateIV();
            return Convert.ToBase64String(aes.IV);
        }
    }
}
