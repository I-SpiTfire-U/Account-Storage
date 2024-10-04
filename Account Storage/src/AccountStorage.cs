using Account_Storage.Accounts;
using Account_Storage.Menus;
using Account_Storage.Utilities;
using Account_Storage.Utilities.Input;

namespace Account_Storage;
internal static class AccountStorage
{
    private static string ConfigFilePath = GetConfigPath();
    private static string SavedAccountsPath = GetSavedAccountsPath();
    private static readonly List<Account> SavedAccounts = AccountIO.ImportAccountsFromFile(SavedAccountsPath);

    internal static void Start()
    {
        Console.Title = "Account Storage";
        Console.CursorVisible = false;

        Console.Clear();
        MainMenu();
    }

    private static string GetConfigPath()
    {
        string path = Path.Combine(Environment.CurrentDirectory, ".config");

        if (!File.Exists(path))
        {
            string encryptedAccountPath = AccountProtection.EncryptString(Path.Combine(Environment.CurrentDirectory, ".accntstrg"));
            File.WriteAllText(path, encryptedAccountPath);
        }

        return path;
    }

    private static string GetSavedAccountsPath()
    {
        string path = AccountProtection.DecryptString(File.ReadAllText(ConfigFilePath));

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }

        return path;
    }

    private static void MainMenu()
    {
        while (true)
        {
            string[] menuItems = [ "Save", "List", "Search", "Create", "Import", "Export", "Path" ];
            int selectedItem = ScrollingMenu.CreateMenu("Account Storage", menuItems);

            switch (selectedItem)
            {
                case 0:
                    AccountIO.ExportAccountsToFile(SavedAccounts, SavedAccountsPath);
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
                    SavedAccounts.AddRange(AccountIO.ImportAccountsFromFile());
                    break;

                case 5:
                    AccountIO.ExportAccountsToFile(SavedAccounts);
                    break;

                case 6:
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
        File.WriteAllText(ConfigFilePath, AccountProtection.EncryptString(SavedAccountsPath));
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
        return new[]
        {
            $"Title : {account.Title}",
            $"Name  : {account.Name}",
            $"Pass  : {account.Pass}",
            $"Email : {account.Email}",
            $"Site  : {account.Site}",
            "Save"
        };
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
