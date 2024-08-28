namespace Account_Storage.Source
{
    internal static class Utilities
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

        internal static string GetValidStringInput(string prompt)
        {
            string? result;

            Console.CursorVisible = true;
            Console.WriteLine(prompt);
            do
            {
                ColorWrite(("> ", false, ConsoleColor.Cyan, null));
                result = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(result));
            Console.CursorVisible = false;
            return result;
        }

        internal static void PrintErrorMessage(string? message)
        {
            ColorWrite((message, false, ConsoleColor.Black, ConsoleColor.Red));
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
