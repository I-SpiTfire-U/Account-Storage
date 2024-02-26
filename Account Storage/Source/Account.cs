namespace Account_Storage.Source
{
    public class Account
    {
        public string Title;
        public string Name;
        public string Password;
        public string Email;
        public string Website;

        public Account(string title, string name, string password, string email, string website)
        {
            Title = title;
            Name = name;
            Password = password;
            Email = email;
            Website = website;
        }

        public Account(string line)
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

        public string GetDisplayString => $"Title: {Title}\n Name: {Name}\n Pass: {Password}\nEmail: {Email}\n Site: {Website}";
    }
}
