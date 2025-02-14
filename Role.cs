using System;
using System.Collections.Generic;

class Program
{
    static Dictionary<string, List<string>> roles = new Dictionary<string, List<string>>
    {
        { "Radiologist", new List<string> { "read", "create", "update", "delete" } },
        { "Physician", new List<string> { "read", "update_comments" } },
        { "LabTechnician", new List<string> { "read_metadata", "update_metadata" } },
        { "Administrator", new List<string> { "read", "create", "update", "delete" } },
        { "Patient", new List<string> { "read_own" } },
        { "BillingStaff", new List<string> { "read_billing" } }
    };

    static Dictionary<string, string> users = new Dictionary<string, string>
    {
        { "User1", "Radiologist" },
        { "User2", "Physician" },
        { "User3", "Patient" }
    };

    static bool HasPermission(string user, string action, Dictionary<string, string> resource = null)
    {
        if (!users.ContainsKey(user))
        {
            Console.WriteLine("User not found.");
            return false;
        }

        string role = users[user];

        if (!roles.ContainsKey(role) || !roles[role].Contains(action))
        {
            Console.WriteLine("Access Denied.");
            return false;
        }

        // Special case: Patients can only read their own reports
        if (action == "read_own" && resource != null && resource.ContainsKey("patient_id"))
        {
            return resource["patient_id"] == user;
        }

        Console.WriteLine("Access Granted.");
        return true;
    }

    static void Main()
    {
        var resource = new Dictionary<string, string> { { "patient_id", "User3" } };

        Console.WriteLine(HasPermission("User3", "read_own", resource)); // True (Patient can read own reports)
        Console.WriteLine(HasPermission("User2", "read", resource)); // True (Physician can read reports)
        Console.WriteLine(HasPermission("User3", "update", resource)); // False (Patient cannot update)
    }
}
