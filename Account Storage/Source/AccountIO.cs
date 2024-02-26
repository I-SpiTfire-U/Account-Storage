namespace Account_Storage.Source
{
    public static class AccountIO
    {
        public static List<Account> ImportAccounts(string path)
        {
            if (!File.Exists(path))
            {
                Utilities.ColorWrite(ConsoleColor.Black, ConsoleColor.DarkRed, "Invalid path.");
                return [];
            }
            Utilities.ColorWrite(ConsoleColor.Black, ConsoleColor.Green, "Accounts imported successfully.");
            return File.ReadLines(path).Select(line => new Account(line)).ToList();
        }

        public static void ExportAccounts(string path, List<Account> accounts)
        {
            if (!path.EndsWith(".txt"))
            {
                Utilities.ColorWrite(ConsoleColor.Black, ConsoleColor.DarkRed, "Invalid path.");
                return;
            }
            Utilities.ColorWrite(ConsoleColor.Black, ConsoleColor.Green, "Accounts exported successfully.");
            File.WriteAllLines(path, accounts.Select(a => a.ToString()));
        }
    }
}
