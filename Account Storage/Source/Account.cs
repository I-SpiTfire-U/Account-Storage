namespace Account_Storage.Source
{
    internal class Account
    {
        internal string Title;
        internal string Name;
        internal string Password;
        internal string Email;
        internal string Website;

        internal Account(string title, string name, string password, string email, string website)
        {
            Title = title;
            Name = name;
            Password = password;
            Email = email;
            Website = website;
        }

        internal Account(string line)
        {
            string[] parts = line.Split('|');
            Title = parts[0];
            Name = parts[1];
            Password = parts[2];
            Email = parts[3];
            Website = parts[4];
        }

        public override string ToString()
        {
            return $"{Title}|{Name}|{Password}|{Email}|{Website}";
        }

        internal string ToDisplayString()
        {
            return $"Title: {Title}\nEmail: {Email}\n Name: {Name}\n Pass: {Password}\n Site: {Website}";
        }
    }
}
