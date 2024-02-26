namespace Account_Storage.Source
{
    public static class Utilities
    {
        public static void ColorWrite(ConsoleColor foreground, ConsoleColor background, object? value)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(value);
            Console.ResetColor();
        }

        public static string GetValidString(string? prompt)
        {
            Console.Clear();
            Console.Write($"{prompt}\n");
            do
            {
                Console.Write("> ");
                prompt = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(prompt));
            return prompt;
        }
    }
}
