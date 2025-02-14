using System;
using System.IO;
using System.Collections.Generic;
using BCrypt.Net;

class Program
{
    static string filePath = "admin_data.txt";
    static int maxLoginAttempts = 3;

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Register Admin");
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

        File.AppendAllText(filePath, $"{username}:{hashedPasscode}\n");
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
        if (!File.Exists(filePath)) return false;

        foreach (string line in File.ReadAllLines(filePath))
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2 && parts[0] == username)
                return true;
        }
        return false;
    }

    static bool VerifyPassword(string username, string passcode)
    {
        if (!File.Exists(filePath)) return false;

        foreach (string line in File.ReadAllLines(filePath))
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
        Console.WriteLine("Access granted to Patient Management System.");
        Console.WriteLine("You can now add and view patient data.");
        // Additional functionalities for patient management can be added here.
    }
}

