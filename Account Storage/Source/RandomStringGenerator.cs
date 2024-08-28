namespace Account_Storage.Source
{
    public static class RandomStringGenerator
    {
        private static readonly Random _Random = new();
        private const string CHARACTER_LIST = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+~`[]{}\/?.><:;'""";

        public static string Generate()
        {
            char[] password = new char[20];
            for (int i = 0; i < 20; i++)
            {
                password[i] = CHARACTER_LIST[_Random.Next(CHARACTER_LIST.Length)];
            }
            return new string(password);
        }
    }
}
