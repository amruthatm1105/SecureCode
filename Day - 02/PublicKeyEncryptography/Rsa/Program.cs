using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

class KeyGenerator
{
    static void Main()
    {
        string privateKeyPath = "private_key.xml";
        string publicKeyPath = "public_key.xml";

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
        {
            File.WriteAllText(privateKeyPath, rsa.ToXmlString(true)); // Private Key
            File.WriteAllText(publicKeyPath, rsa.ToXmlString(false)); // Public Key
        }

        Console.WriteLine("RSA Key Pair Generated Successfully.");
    }
}

