using Account_Storage.Utilities;
using Account_Storage.Utilities.Input;

namespace Account_Storage.Accounts;
internal static class AccountIO
{
    internal static void ExportAccountsToFile(List<Account> accounts, string? path = null)
    {
        string exportPath = path == null ? UserInput.GetStringInput("Export Path") : path;

        if (!OtherUtilities.CheckIfPathIsValid(exportPath))
        {
            OtherUtilities.PrintErrorMessage("Invalid export path entered.");
            return;
        }

        if (accounts.Count == 0)
        {
            OtherUtilities.PrintErrorMessage("No accounts to export.");
            return;
        }

        File.WriteAllLines(exportPath, accounts.Select(account => AccountProtection.EncryptString(account.ToString())));
    }

    internal static List<Account> ImportAccountsFromFile(string? path = null)
    {
        string importPath = path == null ? UserInput.GetStringInput("File Path") : path;

        if (!OtherUtilities.CheckIfPathIsValid(importPath))
        {
            OtherUtilities.PrintErrorMessage("Invalid import path entered.");
            return [];
        }

        List<Account> accounts = [];

        using (StreamReader streamReader = new(importPath))
        {
            string? line;
            while ((line = streamReader.ReadLine()) != null)
            {
                string decryptedLine = AccountProtection.DecryptString(line);
                string[] chunks = decryptedLine.Split('|');
                if (chunks.Length == 5)
                {
                    accounts.Add(new Account(chunks[0], chunks[1], chunks[2], chunks[3], chunks[4]));
                }
            }
        }
        return accounts;
    }
}