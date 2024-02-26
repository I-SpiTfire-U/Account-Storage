namespace Account_Storage.Source
{
    public class Instance
    {
        private string? _SearchTerm;
        private List<Account> _Accounts = [];
        private const string FILEPATH = "Account_List.txt";

        public Instance()
        {
            if (!File.Exists(FILEPATH))
            {
                File.WriteAllText(FILEPATH, "");
            }
            _Accounts.AddRange(AccountIO.ImportAccounts(FILEPATH));
        }

        public void MainMenuLoop()
        {
            Menu mainMenu = new("Select", "Create", "Search", "Import", "Export");
            while (true)
            {
                _Accounts = [.. _Accounts.OrderBy(a => a.Title)];

                switch (mainMenu.StartMenuLoop())
                {
                    case -1:
                        return;
                    case 0:
                        SelectAccount(_Accounts);
                        break;
                    case 1:
                        Account? account = GetNewAccount();
                        if (account != null)
                        {
                            _Accounts.Add(account);
                        }
                        break;
                    case 2:
                        SearchAccounts();
                        break;
                    case 3:
                        _Accounts.AddRange(AccountIO.ImportAccounts(Utilities.GetValidString("Enter Import Path")));
                        _ = Console.ReadKey(true);
                        break;
                    case 4:
                        AccountIO.ExportAccounts(Utilities.GetValidString("Enter Export Path"), _Accounts);
                        _ = Console.ReadKey(true);
                        break;
                }
                AccountIO.ExportAccounts(FILEPATH, _Accounts);
            }
        }

        #region Account Management

        private void SelectAccount(List<Account> accounts)
        {
            if (_Accounts.Count == 0)
            {
                Console.Clear();
                Utilities.ColorWrite(ConsoleColor.Black, ConsoleColor.DarkRed, "No available accounts to preform actions on!");
                _ = Console.ReadKey(true);
                return;
            }

            Menu optionMenu = new("View", "Edit", "Delete");
            Menu accountMenu = new(accounts.Select(a => a.Title).ToArray());

            while (true)
            {
                int index = accountMenu.StartMenuLoop("Back");

                if (index == -1)
                {
                    return;
                }

                int optionIndex = optionMenu.StartMenuLoop("Back");

                switch (optionIndex)
                {
                    case 0:
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(accounts[index].GetDisplayString);
                        _ = Console.ReadKey(true);
                        continue;
                    case 1:
                        Account? account = GetNewAccount(index);
                        if (account != null)
                        {
                            _Accounts[index] = account;
                        }
                        break;
                    case 2:
                        _ = _Accounts.Remove(accounts[index]);
                        accounts.RemoveAt(index);
                        break;
                }
                accountMenu.Items = accounts.Select(a => a.Title).ToArray();
            }
        }

        private Account? GetNewAccount(int index = -1)
        {
            Account account = index == -1 ? new Account("||||") : _Accounts[index];
            Menu menu = new("Title", "Name", "Password", "Email", "Website", "Save");

            while (true)
            {
                switch (menu.StartMenuLoop("Back"))
                {
                    case -1:
                        return null;
                    case 0:
                        account.Title = Utilities.GetValidString($"Current Title: {account.Title}");
                        break;
                    case 1:
                        account.Name = Utilities.GetValidString($"Current Name: {account.Name}");
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Generate Random Password? Y/N");
                        account.Password = Console.ReadKey(true).Key == ConsoleKey.Y ? RandomStringGenerator.Generate() : Utilities.GetValidString($"Current Password: {account.Password}");
                        break;
                    case 3:
                        account.Email = Utilities.GetValidString($"Current Email: {account.Email}");
                        break;
                    case 4:
                        account.Website = Utilities.GetValidString($"Current Website: {account.Website}");
                        break;
                    case 5:
                        return account;

                }
            }
        }

        private void SearchAccounts()
        {
            _SearchTerm = Utilities.GetValidString("Search Accounts");
            List<Account> accounts = _Accounts.Where(a => a.Title.Contains(_SearchTerm)).ToList();
            SelectAccount(accounts);
        }

        #endregion
    }
}