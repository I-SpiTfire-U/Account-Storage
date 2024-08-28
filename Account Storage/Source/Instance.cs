namespace Account_Storage.Source
{
    internal class Instance
    {
        private readonly List<Account> SavedAccounts;
        private const string SavedAccountsPath = "SavedAccounts.txt";

        internal Instance()
        {
            if (!Path.Exists(SavedAccountsPath))
            {
                File.WriteAllText(SavedAccountsPath, "");
            }
            SavedAccounts = AccountIO.ImportAccountsFromFile(SavedAccountsPath);
        }

        internal void Start()
        {
            Console.Title = "Account Storage";
            Console.CursorVisible = false;
            Console.Clear();
            MainMenu();
        }

        private void MainMenu()
        {
            while (true)
            {
                switch (Menus.CreateBasicMenu("Account Storage", ["Save", "List", "Search", "Create", "Import", "Export"]))
                {
                    case 0:
                        AccountIO.ExportAccountsToFile(SavedAccountsPath, SavedAccounts);
                        break;
                    case 1:
                        SavedAccountsMenu(null);
                        break;
                    case 2:
                        SavedAccountsMenu(Utilities.GetValidStringInput("Enter Search Term"));
                        break;
                    case 3:
                        Account? newAccount = CreateNewAccount();
                        if (newAccount != null)
                        {
                            SavedAccounts.Add(newAccount);
                        }
                        break;
                    case 4:
                        string importPath = Utilities.GetValidStringInput("File Path");
                        if (string.IsNullOrWhiteSpace(importPath) || !Path.Exists(importPath))
                        {
                            Utilities.PrintErrorMessage("Invalid import path given.");
                            break;
                        }
                        SavedAccounts.AddRange(AccountIO.ImportAccountsFromFile(importPath));
                        break;
                    case 5:
                        string exportPath = Utilities.GetValidStringInput("File Path");
                        if (string.IsNullOrWhiteSpace(exportPath) || !Path.Exists(exportPath))
                        {
                            Utilities.PrintErrorMessage("Invalid export path given.");
                            break;
                        }
                        AccountIO.ExportAccountsToFile(exportPath, SavedAccounts);
                        break;
                    case -1:
                        return;
                }
            }
        }

        private void SavedAccountsMenu(string? searchTerm)
        {
            while (true)
            {
                IEnumerable<Account> filteredAccounts = searchTerm != null ? SavedAccounts.Where(a => a.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) : SavedAccounts;
                List<Account> tempAccounts = [.. filteredAccounts.OrderBy(account => account.Title, StringComparer.OrdinalIgnoreCase)];

                if (tempAccounts.Count == 0)
                {
                    Utilities.PrintErrorMessage($"No accounts found.");
                    break;
                }

                Account? selectedAccount = Menus.CreateAccountMenu("List Of Accounts", [.. tempAccounts]);
                if (selectedAccount == null)
                {
                    return;
                }
                AccountOptionsMenu(selectedAccount);
            }
        }

        private void AccountOptionsMenu(Account selectedAccount)
        {
            while (true)
            {
                switch (Menus.CreateBasicMenu($"Options For: {selectedAccount.Title}", ["View", "Edit", "Delete"]))
                {
                    case 0:
                        selectedAccount.DisplayAccountInformation();
                        break;
                    case 1:
                        Account? newAccount = CreateNewAccount(selectedAccount);
                        if (newAccount != null)
                        {
                            SavedAccounts.Remove(selectedAccount);
                            SavedAccounts.Add(newAccount);
                            selectedAccount = newAccount;
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

        private static Account? CreateNewAccount(Account? existingAccount = null)
        {
            Account account = existingAccount != null ? new Account(existingAccount) : new Account();

            while (true)
            {
                switch (Menus.CreateBasicMenu("Account", [$"Title    : {account.Title}", $"Username : {account.Name}", $"Password : {account.Pass}", $"Email    : {account.Email}", $"Website  : {account.Site}", "Save"]))
                {
                    case 0:
                        account.Title = GetNewAccountValue("Title", account.Title);
                        break;
                    case 1:
                        account.Name = GetNewAccountValue("Username", account.Name);
                        break;
                    case 2:
                        Console.Write("Generate random password? [y/n]");
                        account.Pass = Console.ReadKey(true).KeyChar == 'y' ? RandomStringGenerator.Generate() : GetNewAccountValue("Password", account.Pass);
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

        private static string GetNewAccountValue(string type, string previousValue)
        {
            return Utilities.GetValidStringInput($"{type} [Previous: {previousValue}]");
        }
    }
}