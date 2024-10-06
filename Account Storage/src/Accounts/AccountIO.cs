using Account_Storage.Utilities;
using Account_Storage.Utilities.Input;

namespace Account_Storage.Accounts;
internal static class AccountIO
{
    internal static void ExportAccountsToFile(List<Account> accounts, byte[] key, byte[] iv, string? path = null)
    {
        string exportPath = path ?? UserInput.GetStringInput("Export Path");

        if (accounts.Count == 0)
        {
            OtherUtilities.PrintErrorMessage("No accounts to export.");
            return;
        }

        File.WriteAllLines(exportPath, accounts.Select(account => account.ToString()));
        AccountEncryption.EncryptFile(key, iv, exportPath);
    }

    internal static List<Account> ImportAccountsFromFile(byte[] key, byte[] iv, string? path = null)
    {
        string importPath = path ?? UserInput.GetStringInput("Import Path");

        if (!OtherUtilities.CheckIfPathIsValid(importPath))
        {
            OtherUtilities.PrintErrorMessage("Invalid import path entered.");
            return [];
        }

        List<Account> accounts = [];

        try
        {
            AccountEncryption.DecryptFile(key, iv, importPath);
        }
        catch
        {
            OtherUtilities.PrintErrorMessage("Decryption failed. Invalid key or IV");
            return [];
        }

        using (StreamReader streamReader = new(importPath))
        {
            string? line;
            while ((line = streamReader.ReadLine()) != null)
            {
                string[] chunks = line.Split('|');
                if (chunks.Length == 5)
                {
                    accounts.Add(new Account(chunks[0], chunks[1], chunks[2], chunks[3], chunks[4]));
                }
            }
        }
        AccountEncryption.EncryptFile(key, iv, importPath);
        return accounts;
    }

    internal static List<Account> ImportUnencryptedAccountsFromFile(string? path = null)
    {
        string importPath = path ?? UserInput.GetStringInput("Import Path");

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
                string[] chunks = line.Split('|');
                if (chunks.Length == 5)
                {
                    accounts.Add(new Account(chunks[0], chunks[1], chunks[2], chunks[3], chunks[4]));
                }
            }
        }
        return accounts;
    }
}