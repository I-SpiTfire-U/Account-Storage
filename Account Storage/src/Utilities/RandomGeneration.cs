namespace Account_Storage.Utilities;
public static class RandomGeneration
{
    private static readonly Random _Random = new();
    private const string _CharacterList = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+~`[]{}\\/?.><:;'\"";

    public static string GenerateString(int length)
    {
        char[] password = new char[length];
        for (int i = 0; i < length; i++)
        {
            password[i] = _CharacterList[_Random.Next(_CharacterList.Length)];
        }
        return new string(password);
    }
}