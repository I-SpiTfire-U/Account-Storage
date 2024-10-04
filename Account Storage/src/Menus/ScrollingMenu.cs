using System.Text;
using Account_Storage.Utilities;

namespace Account_Storage.Menus;
internal static class ScrollingMenu
{
    internal static int CreateMenu(string title, IEnumerable<string> items)
    {
        string[] paddedItems = GetPaddedItems(items);
        int currentIndex = 0;
        int currentScroll = 0;
        int currentIndexAndScroll = 0;
        int numberOfItemsToDisplay = Math.Min(paddedItems.Length, Console.WindowHeight - 3);

        Console.Clear();
        while (true)
        {
            DisplayMenu(title, paddedItems, currentIndex, currentScroll, numberOfItemsToDisplay);
            currentIndexAndScroll = currentIndex + currentScroll;

            char charInput = Console.ReadKey(true).KeyChar;
            if (charInput == 'a')
            {
                Console.Clear();
                return currentIndexAndScroll;
            }

            if (charInput == 'z')
            {
                Console.Clear();
                return -1;
            }

            if (charInput == 'w' && currentIndexAndScroll > 0)
            {
                if (currentIndex <= 0 && currentScroll > 0)
                {
                    currentScroll--;
                }
                else if (currentIndex > 0)
                {
                    currentIndex--;
                }
            }

            if (charInput == 'd' && currentIndexAndScroll < paddedItems.Length)
            {
                if (currentIndex == numberOfItemsToDisplay - 1 && currentIndexAndScroll < paddedItems.Length - 1)
                {
                    currentScroll++;
                }
                else if (currentIndex < numberOfItemsToDisplay - 1)
                {
                    currentIndex++;
                }
            }
        }
    }
    private static void DisplayMenu(string title, string[] items, int currentIndex, int currentScroll, int numberOfItemsToDisplay)
    {
        StringBuilder menu = new();
        
        for (int i = 0; i < numberOfItemsToDisplay; i++)
        {
            string currentOption = items[i + currentScroll];
            menu.AppendLine(i == currentIndex ? $"> {currentOption}" : $"  {currentOption}");
        }
        Console.SetCursorPosition(0, 0);
        OtherUtilities.ColorWrite((title, true, ConsoleColor.Green, null));
        Console.Write(menu.ToString());

        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        OtherUtilities.ColorWrite(($"[w] UP [d] DOWN [a] SELECT [z] BACK/EXIT <{items.Length} Items>", false, ConsoleColor.Black, ConsoleColor.Blue));
    }
    private static string[] GetPaddedItems(IEnumerable<string> items)
    {
        string[] result = new string[items.Count()];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = items.ElementAt(i).PadRight(Console.WindowWidth - 2);
        }
        return result;
    }
}