//
// Just a quick note:
// -----------------------------------------------------------------------------------------------
// This file is a massive mess right now and I plan on going through and updating it eventually. 
// If you were planning on going through the source code of this project, consider this a warning.
// -----------------------------------------------------------------------------------------------
//
// Have fun! :3
//

using System.Security.Cryptography;
using Account_Storage.Accounts;
using Account_Storage.Menus;
using Account_Storage.Utilities;
using Account_Storage.Utilities.Input;

namespace Account_Storage;
internal static class AccountStorage
{
    private static bool IsFirstLogin = false;
    private static string SavedAccountsPath;
    private static readonly string ConfigFilePath;
    private static readonly byte[] EncryptionKey;
    private static readonly byte[] EncryptionIV;
    private static readonly List<Account> SavedAccounts;

    static AccountStorage()
    {
        InitializeConsole();
        
        ConfigFilePath = GetConfigPath();
        (EncryptionKey, EncryptionIV) = IsFirstLogin ? GetNewKeyAndIV() : GetUserKeyAndIV();
        SavedAccountsPath = GetSavedAccountsPath();
        SavedAccounts = AccountIO.ImportAccountsFromFile(EncryptionKey, EncryptionIV, SavedAccountsPath);
    }

    internal static void Start()
    {
        Console.Clear();
        MainMenu();
    }

    private static void InitializeConsole()
    {
        Console.Title = "Account Storage";
        Console.CursorVisible = false;
        Console.Clear();
    }

    private static string GetConfigPath()
    {
        string path = Path.Combine(Environment.CurrentDirectory, ".config");

        if (!File.Exists(path))
        {
            string accountsPath = Path.Combine(Environment.CurrentDirectory, ".accntstrg");
            File.WriteAllText(path, accountsPath);
            IsFirstLogin = true;
        }

        return path;
    }

    private static string GetSavedAccountsPath()
    {
        string path = File.ReadAllText(ConfigFilePath);

        if (!File.Exists(path))
        {
            using (File.Create(path)) {}
            AccountEncryption.EncryptFile(EncryptionKey, EncryptionIV, path);
        }

        return path;
    }

    private static (byte[] key, byte[] iv) GetNewKeyAndIV()
    {
        using Aes aes = Aes.Create();
        aes.GenerateKey();
        aes.GenerateIV();

        byte[] key = aes.Key;
        byte[] iv = aes.IV;

        OtherUtilities.ColorWrite(("SAVE YOUR KEY AND IV IN A SAFE SPOT!\nYOU WILL NEED THEM TO DECRYPT YOUR ACCOUNTS.\n", true, ConsoleColor.Red, null));
        Console.WriteLine($"KEY={Convert.ToHexString(key)}\n IV={Convert.ToHexString(iv)}");
        
        PromptForConfirmation();

        return (key, iv);
    }

    private static void PromptForConfirmation()
    {
        OtherUtilities.ColorWrite(("\nTYPE YES ONCE YOU HAVE SAVED YOUR KEY AND IV IN A SAFE SPOT!", true, ConsoleColor.Red, null));
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && input.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
            {
                break;
            }
        }
    }

    private static (byte[] key, byte[] iv) GetUserKeyAndIV()
    {
        Console.WriteLine("Login\n---------------");

        byte[] key = Convert.FromHexString(UserInput.GetStringInput("Encryption Key"));
        byte[] iv = Convert.FromHexString(UserInput.GetStringInput("\nEncryption IV"));

        if (!AccountEncryption.AreKeyAndIVCorrect(SavedAccountsPath, key, iv))
        {
            OtherUtilities.PrintErrorMessage("Invalid key or iv entered.");
            Environment.Exit(0);
        }

        return (key, iv);
    }

    private static void MainMenu()
    {
        while (true)
        {
            string[] menuItems = [ "Save", "List", "Search", "Create", "Import Encrypted", "Import Plaintext", "Export", "Change Path" ];
            int selectedItem = ScrollingMenu.CreateMenu("Account Storage", menuItems);

            switch (selectedItem)
            {
                case 0:
                    AccountIO.ExportAccountsToFile(SavedAccounts, EncryptionKey, EncryptionIV, SavedAccountsPath);
                    break;

                case 1:
                    SavedAccountsMenu();
                    break;

                case 2:
                    SavedAccountsMenu(UserInput.GetStringInput("Enter Search Term"));
                    break;

                case 3:
                    Account? newAccount = CreateNewAccount();
                    if (newAccount != null)
                    {
                        SavedAccounts.Add(newAccount);
                    }
                    break;

                case 4:
                    SavedAccounts.AddRange(AccountIO.ImportAccountsFromFile(EncryptionKey, EncryptionIV));
                    break;

                case 5:
                    SavedAccounts.AddRange(AccountIO.ImportUnencryptedAccountsFromFile());
                    break;

                case 6:
                    AccountIO.ExportAccountsToFile(SavedAccounts, EncryptionKey, EncryptionIV);
                    break;

                case 7:
                    UpdateAccountsPath();
                    break;

                case -1:
                    return;
            }
        }
    }

    private static void UpdateAccountsPath()
    {
        string newPath = UserInput.GetStringInput("New Path");

        if (!OtherUtilities.CheckIfPathIsValid(newPath))
        {
            OtherUtilities.PrintErrorMessage("Invalid path given.");
            return;
        }

        string newSavedAccountsPath = Path.Combine(newPath, ".accntstrg");
        File.Move(SavedAccountsPath, newSavedAccountsPath);

        SavedAccountsPath = newSavedAccountsPath;
        File.WriteAllText(ConfigFilePath, SavedAccountsPath);
    }

    private static void SavedAccountsMenu(string? searchTerm = null)
    {
        while (true)
        {
            List<Account> tempAccounts = [.. SavedAccounts
                .Where(item => searchTerm == null || item.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .OrderBy(item => item.Title, StringComparer.OrdinalIgnoreCase)];

            if (tempAccounts.Count == 0)
            {
                OtherUtilities.PrintErrorMessage($"No accounts found.");
                break;
            }

            int selectedAccount = ScrollingMenu.CreateMenu("List Of Accounts", tempAccounts.Select(item => item.Title));
            if (selectedAccount == -1)
            {
                return;
            }
            
            AccountOptionsMenu(tempAccounts[selectedAccount]);
        }
    }
    
    private static void AccountOptionsMenu(Account selectedAccount)
    {
        while (true)
        {
            string[] menuItems = [ "View", "Edit", "Delete" ];
            int selectedItem = ScrollingMenu.CreateMenu($"Options For: {selectedAccount.Title}", menuItems);

            switch (selectedItem)
            {
                case 0:
                    selectedAccount.DisplayAccountInformation();
                    break;

                case 1:
                    Account? updatedAccount = CreateNewAccount(selectedAccount);
                    if (updatedAccount != null)
                    {
                        UpdateAccountList(selectedAccount, updatedAccount);
                        selectedAccount = updatedAccount;
                    }
                    break;

                case 2:
                    SavedAccounts.Remove(selectedAccount);
                    return;

                case -1:
                    return;
            }
        }
    }

    private static void UpdateAccountList(Account newAccount, Account oldAccount)
    {
        SavedAccounts.Remove(oldAccount);
        SavedAccounts.Add(newAccount);
    }

    private static Account? CreateNewAccount(Account? existingAccount = null)
    {
        Account account = existingAccount != null ? new(existingAccount) : new();

        while (true)
        {
            int selectedItem = ScrollingMenu.CreateMenu("New Account", GetAccountMenuOptions(account));

            switch (selectedItem)
            {
                case 0:
                    account.Title = GetNewAccountValue("Title", account.Title);
                    break;
                case 1:
                    account.Name = GetNewAccountValue("Username", account.Name);
                    break;
                case 2:
                    account.Pass = GetPassword(account.Pass);
                    break;
                case 3:
                    account.Email = GetNewAccountValue("Email", account.Email);
                    break;
                case 4:
                    account.Site = GetNewAccountValue("Website", account.Site);
                    break;
                case 5:
                    return account;
                case -1:
                    return null;
            }
        }
    }

    private static string[] GetAccountMenuOptions(Account account)
    {
        return
        [
            $"Title : {account.Title}",
            $"Name  : {account.Name}",
            $"Pass  : {account.Pass}",
            $"Email : {account.Email}",
            $"Site  : {account.Site}",
            "Save"
        ];
    }

    private static string GetPassword(string previousPassword)
    {
        return UserInput.GetYesNoInput("Generate a random password?")
            ? RandomGeneration.GenerateString(UserInput.GetNumberInput("Password length"))
            : GetNewAccountValue("Password", previousPassword);
    }

    private static string GetNewAccountValue(string type, string previousValue)
    {
        return UserInput.GetStringInput($"{type} [Previous: {previousValue}]");
    }
}
