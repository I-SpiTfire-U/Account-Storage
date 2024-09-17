namespace Account_Storage.Source
{
    public static class RandomStringGenerator
    {
        private static readonly Random _Random = new();
        private const string _CharacterList = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+~`[]{}\/?.><:;'""";

        public static string Generate()
        {
            char[] password = new char[20];
            for (int i = 0; i < 20; i++)
            {
                password[i] = _CharacterList[_Random.Next(_CharacterList.Length)];
            }
            return new string(password);
        }
    }
}
