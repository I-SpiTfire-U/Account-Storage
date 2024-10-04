using Account_Storage.Utilities;

namespace Account_Storage.Accounts;
internal class Account
{
    internal string Title = "";
    internal string Name = "";
    internal string Pass = "";
    internal string Email = "";
    internal string Site = "";

    internal Account() {}

    internal Account(string title, string name, string pass, string email, string site)
    {
        Title = title;
        Name  = name;
        Pass  = pass;
        Email = email;
        Site  = site;
    }

    internal Account(string fullAccount)
    {
        string[] chunks = fullAccount.Split('|');
        Title = chunks[0];
        Name  = chunks[1];
        Pass  = chunks[2];
        Email = chunks[3];
        Site  = chunks[4];
    }

    internal Account(Account existingAccount)
    {
        Title = existingAccount.Title;
        Name  = existingAccount.Name;
        Pass  = existingAccount.Pass;
        Email = existingAccount.Email;
        Site  = existingAccount.Site;
    }

    internal void DisplayAccountInformation()
    {
        Console.Clear();

        OtherUtilities.ColorWrite((Title, true, ConsoleColor.Green, null));
        Console.WriteLine(new string('-', Title.Length + 1));
        OtherUtilities.ColorWrite
        (
            ("username: ", false, ConsoleColor.Blue, ConsoleColor.Black), (Name, true, null, null),
            ("password: ", false, ConsoleColor.Blue, ConsoleColor.Black), (Pass, true, null, null),
            (" website: ", false, ConsoleColor.Blue, ConsoleColor.Black), (Site, true, null, null),
            ("   Email: ", false, ConsoleColor.Blue, ConsoleColor.Black), (Email, true, null, null)
        );

        Console.ReadKey(true);
    }

    public override string ToString()
    {
        return $"{Title}|{Name}|{Pass}|{Email}|{Site}";
    }
}