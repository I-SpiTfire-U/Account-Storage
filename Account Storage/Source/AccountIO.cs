namespace Account_Storage.Source
{
    internal static class AccountIO
    {
        internal static void ExportAccountsToFile(string path, List<Account> accounts)
        {
            if (accounts.Count == 0)
            {
                Utilities.PrintErrorMessage("No accounts to export.");
                return;
            }

            File.WriteAllLines(path, accounts.Select(account => AccountProtection.Encrypt(account.ToString())));
        }

        internal static List<Account> ImportAccountsFromFile(string path)
        {
            List<Account> accounts = [];

            using (StreamReader streamReader = new(path))
            {
                string? line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    string decryptedLine = AccountProtection.Decrypt(line);
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
}