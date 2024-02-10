namespace Account_Storage.Source
{
    internal class Program
    {
        private static string? _SearchTerm = "";
        private static List<Account> _Accounts = [];
        private const string _CharacterList = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+~`[]{}\/?.><:;'""";
        private const string _FilePath = "Account_List.txt";

        internal static void Main()
        {
            if (!File.Exists(_FilePath))
            {
                File.WriteAllText(_FilePath, "");
            }
            LoadAccountsFromFile(_FilePath);

            Menu mainMenu = new("Select", "Create", "Search", "Import", "Export", "Save");
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
                        CreateNewAccount();
                        break;
                    case 2:
                        SearchAccounts();
                        break;
                    case 3:
                        ImportAccounts(GetValidStringInput("Enter Import Path"));
                        break;
                    case 4:
                        ExportAccounts(GetValidStringInput("Enter Export Path"));
                        break;
                    case 5:
                        Console.Clear();
                        ExportAccounts(_FilePath);
                        break;
                }
            }
        }

        public static string GetValidStringInput(string? prompt)
        {
            ClearWriteLine(prompt + "\n");
            do
            {
                Console.Write("> ");
                prompt = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(prompt));
            return prompt;
        }

        public static void ColorWrite(ConsoleColor foreground, ConsoleColor backgroundm, object? value)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = backgroundm;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void ClearWriteLine(object? value)
        {
            Console.Clear();
            Console.Write(value);
        }

        private static string GeneratePassword()
        {
            char[] password = new char[20];
            for (int i = 0; i < 20; i++)
            {
                password[i] = _CharacterList[new Random().Next(_CharacterList.Length)];
            }
            return new string(password);
        }

        #region File Management

        private static void ImportAccounts(string path)
        {
            if (File.Exists(path))
            {
                LoadAccountsFromFile(path);
                ColorWrite(ConsoleColor.Black, ConsoleColor.Green, "Accounts imported successfully.");
                _ = Console.ReadKey(true);
                return;
            }
            ColorWrite(ConsoleColor.Black, ConsoleColor.DarkRed, "Invalid path given.");
            _ = Console.ReadKey(true);
        }

        private static void ExportAccounts(string path)
        {
            if (path.EndsWith(".txt"))
            {
                File.WriteAllLines(path, _Accounts.Select(a => a.ToString()));
                ColorWrite(ConsoleColor.Black, ConsoleColor.Green, "Accounts exported successfully.");
                _ = Console.ReadKey(true);
                return;
            }
            ColorWrite(ConsoleColor.Black, ConsoleColor.DarkRed, "Invalid path given.");
            _ = Console.ReadKey(true);
        }

        private static void LoadAccountsFromFile(string path)
        {
            foreach (string s in File.ReadAllLines(path))
            {
                _Accounts.Add(new Account(s));
            }
        }

        #endregion

        #region Account Management

        private static void SelectAccount(List<Account> accounts)
        {
            if (_Accounts.Count == 0)
            {
                Console.Clear();
                ColorWrite(ConsoleColor.Black, ConsoleColor.DarkRed, "No available accounts to preform actions on!");
                _ = Console.ReadKey(true);
                return;
            }

            Menu optionMenu = new("View", "Edit", "Delete");
            Menu accountMenu = new(accounts.Select(a => a.Title).ToArray());

            while (true)
            {
                int index = accountMenu.StartMenuLoop();

                if (index == -1)
                {
                    return;
                }

                int optionIndex = optionMenu.StartMenuLoop();

                switch (optionIndex)
                {
                    case 0:
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(accounts[index].ToDisplayString());
                        _ = Console.ReadKey(true);
                        continue;
                    case 1:
                        EditAccount(index);
                        break;
                    case 2:
                        _ = _Accounts.Remove(accounts[index]);
                        accounts.RemoveAt(index);
                        break;
                }
                accountMenu.Items = accounts.Select(a => a.Title).ToArray();
            }
        }

        private static void CreateNewAccount()
        {
            string title = GetValidStringInput($"Enter Title");
            string name = GetValidStringInput($"Enter Name");
            ClearWriteLine("Generate Random Password? Y/N");
            string password = Console.ReadKey().Key == ConsoleKey.Y ? GeneratePassword() : GetValidStringInput($"Enter Password");
            string email = GetValidStringInput($"Enter Email");
            string website = GetValidStringInput($"Enter Website");

            _Accounts.Add(new Account(title, name, password, email, website));
        }

        private static void EditAccount(int index)
        {
            Menu menu = new("Title", "Name", "Password", "Email", "Website");

            while (true)
            {
                switch (menu.StartMenuLoop())
                {
                    case -1:
                        return;
                    case 0:
                        _Accounts[index].Title = GetValidStringInput($"Current Title: {_Accounts[index].Title}");
                        break;
                    case 1:
                        _Accounts[index].Name = GetValidStringInput($"Current Name: {_Accounts[index].Name}");
                        break;
                    case 2:
                        ClearWriteLine("Generate Random Password? Y/N");
                        _Accounts[index].Password = Console.ReadKey(true).Key == ConsoleKey.Y ? GeneratePassword() : GetValidStringInput($"Current Password: {_Accounts[index].Password}");
                        break;
                    case 3:
                        _Accounts[index].Email = GetValidStringInput($"Current Email: {_Accounts[index].Email}");
                        break;
                    case 4:
                        _Accounts[index].Website = GetValidStringInput($"Current Website: {_Accounts[index].Website}");
                        break;
                }
            }
        }

        private static void SearchAccounts()
        {
            _SearchTerm = GetValidStringInput("Search Accounts");

            List<Account> accounts = _Accounts.Where(a => a.Title.Contains(_SearchTerm)).ToList();
            SelectAccount(accounts);

            _SearchTerm = string.Empty;
        }

        #endregion
    }
}