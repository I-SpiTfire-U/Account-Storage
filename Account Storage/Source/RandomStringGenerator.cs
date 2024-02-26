namespace Account_Storage.Source
{
    public static class RandomStringGenerator
    {
        private static readonly Random _Random = new();
        private const string CHARACTERLIST = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+~`[]{}\/?.><:;'""";

        public static string Generate()
        {
            char[] password = new char[20];
            for (int i = 0; i < 20; i++)
            {
                password[i] = CHARACTERLIST[_Random.Next(CHARACTERLIST.Length)];
            }
            return new string(password);
        }
    }
}
