namespace Account_Storage.Utilities;

internal static class OtherUtilities
{
    internal static void ColorWrite(params (string? value, bool addNewline, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)[] lines)
    {
        ConsoleColor originalForegroundColor = Console.ForegroundColor;
        ConsoleColor originalBackgroundColor = Console.BackgroundColor;

        foreach (var (value, addNewline, foregroundColor, backgroundColor) in lines)
        {
            if (foregroundColor.HasValue)
            {
                Console.ForegroundColor = foregroundColor.Value;
            }
            if (backgroundColor.HasValue)
            {
                Console.BackgroundColor = backgroundColor.Value;
            }

            if (addNewline)
            {
                Console.WriteLine(value);
            }
            else
            {
                Console.Write(value);
            }

            Console.ForegroundColor = originalForegroundColor;
            Console.BackgroundColor = originalBackgroundColor;
        }
    }

    internal static void PrintErrorMessage(string? message)
    {
        ColorWrite((message, false, ConsoleColor.Black, ConsoleColor.Red));
        Console.ReadKey(true);
        Console.Clear();
    }
    
    internal static bool CheckIfPathIsValid(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !Path.Exists(path))
        {
            PrintErrorMessage("Invalid path given.");
            return false;
        }
        return true;
    }
}