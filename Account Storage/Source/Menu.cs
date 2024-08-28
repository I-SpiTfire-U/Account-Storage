using System.Text;

namespace Account_Storage.Source
{
    internal static class Menus
    {
        internal static int CreateBasicMenu(string title, string[] items)
        {
            int currentIndex = 0;
            int currentScrollAmount = 0;
            int currentIndexPlusScroll = 0;
            int numberOfItemsToDisplay = Math.Min(items.Length, Console.WindowHeight - 3);
            ConsoleKey keyInput;

            items = AddPaddingToOptions(items);

            Console.Clear();
            DisplayMenu(title, items, currentIndex, currentScrollAmount, numberOfItemsToDisplay);

            while (true)
            {
                keyInput = Console.ReadKey(true).Key;

                if (keyInput == ConsoleKey.A)
                {
                    Console.Clear();
                    return currentIndexPlusScroll;
                }
                else if (keyInput == ConsoleKey.Z)
                {
                    Console.Clear();
                    return -1;
                }
                else if (keyInput == ConsoleKey.W && currentIndexPlusScroll > 0)
                {
                    if (currentIndex <= 0 && currentScrollAmount > 0)
                    {
                        currentScrollAmount--;
                    }
                    else if (currentIndex > 0)
                    {
                        currentIndex--;
                    }
                }
                else if (keyInput == ConsoleKey.D && currentIndexPlusScroll < items.Length)
                {
                    if (currentIndex == numberOfItemsToDisplay - 1 && currentIndexPlusScroll < items.Length - 1)
                    {
                        currentScrollAmount++;
                    }
                    else if (currentIndex < numberOfItemsToDisplay - 1)
                    {
                        currentIndex++;
                    }
                }
                else
                {
                    continue;
                }

                currentIndexPlusScroll = currentIndex + currentScrollAmount;
                DisplayMenu(title, items, currentIndex, currentScrollAmount, numberOfItemsToDisplay);
            }
        }

        internal static Account? CreateAccountMenu(string title, Account[] items)
        {
            string[] itemStrings = items.Select(account => account.Title).ToArray();
            int currentIndex = 0;
            int currentScrollAmount = 0;
            int currentIndexPlusScroll = 0;
            int numberOfItemsToDisplay = Math.Min(items.Length, Console.WindowHeight - 3);
            ConsoleKey keyInput;

            itemStrings = AddPaddingToOptions(itemStrings);

            Console.Clear();
            DisplayMenu(title, itemStrings, currentIndex, currentScrollAmount, numberOfItemsToDisplay);

            while (true)
            {
                keyInput = Console.ReadKey(true).Key;

                if (keyInput == ConsoleKey.A)
                {
                    Console.Clear();
                    return items[currentIndexPlusScroll];
                }
                else if (keyInput == ConsoleKey.Z)
                {
                    Console.Clear();
                    return null;
                }
                else if (keyInput == ConsoleKey.W && currentIndexPlusScroll > 0)
                {
                    if (currentIndex <= 0 && currentScrollAmount > 0)
                    {
                        currentScrollAmount--;
                    }
                    else if (currentIndex > 0)
                    {
                        currentIndex--;
                    }
                }
                else if (keyInput == ConsoleKey.D && currentIndexPlusScroll < items.Length)
                {
                    if (currentIndex == numberOfItemsToDisplay - 1 && currentIndexPlusScroll < items.Length - 1)
                    {
                        currentScrollAmount++;
                    }
                    else if (currentIndex < numberOfItemsToDisplay - 1)
                    {
                        currentIndex++;
                    }
                }
                else
                {
                    continue;
                }

                currentIndexPlusScroll = currentIndex + currentScrollAmount;
                DisplayMenu(title, itemStrings, currentIndex, currentScrollAmount, numberOfItemsToDisplay);
            }
        }

        private static void DisplayMenu(string title, string[] items, int currentIndex, int currentScrollAmount, int numberOfItemsToDisplay)
        {
            StringBuilder menu = new();
            string currentOption;
            
            for (int i = 0; i < numberOfItemsToDisplay; i++)
            {
                currentOption = items[i + currentScrollAmount];
                menu.AppendLine(i == currentIndex ? $"> {currentOption}" : $"  {currentOption}");
            }

            Console.SetCursorPosition(0, 0);
            Utilities.ColorWrite((title, true, ConsoleColor.Green, null));
            Console.Write(menu.ToString());

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Utilities.ColorWrite(("[W] Up [D] Down [A] Select [Z] Back/Exit", false, ConsoleColor.Black, ConsoleColor.Blue));
        }

        private static string[] AddPaddingToOptions(string[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = items[i].PadRight(Console.WindowWidth - 2);
            }
            return items;
        }
    }
}
