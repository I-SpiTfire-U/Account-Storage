namespace Account_Storage.Utilities.Input;
internal static class UserInput
{
    internal static string GetStringInput(string prompt)
    {
        Console.CursorVisible = true;
        Console.WriteLine(prompt);

        while (true)
        {
            OtherUtilities.ColorWrite(("> ", false, ConsoleColor.Cyan, null));

            string? result = Console.ReadLine();
            if (!string.IsNullOrEmpty(result))
            {
                Console.CursorVisible = false;
                return result;
            }
        }
    }

    internal static int GetNumberInput(string prompt)
    {
        Console.CursorVisible = true;
        Console.WriteLine(prompt);

        while (true)
        {
            OtherUtilities.ColorWrite(("> ", false, ConsoleColor.Cyan, null));

            if (int.TryParse(Console.ReadLine(), out int result))
            {
                Console.CursorVisible = false;
                return result;
            }
        }
    }

    internal static bool GetYesNoInput(string prompt)
    {
        Console.WriteLine($"{prompt} [y/n]");

        while (true)
        {
            char input = Console.ReadKey(true).KeyChar;
            
            if (input == 'y')
            {
                return true;
            }
            if (input == 'n')
            {
                return false;
            }
        }
    }
}