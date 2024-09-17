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
            char charInput;

            items = AddPaddingToOptions(items);

            Console.Clear();
            while (true)
            {
                DisplayMenu(title, items, currentIndex, currentScrollAmount, numberOfItemsToDisplay, false);
                charInput = Console.ReadKey(true).KeyChar;

                if (charInput == 'a')
                {
                    Console.Clear();
                    return currentIndexPlusScroll;
                }
                else if (charInput == 'z')
                {
                    Console.Clear();
                    return -1;
                }
                else if (charInput == 'w' && currentIndexPlusScroll > 0)
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
                else if (charInput == 'd' && currentIndexPlusScroll < items.Length)
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
            }
        }

        internal static Account? CreateAccountMenu(string title, Account[] items)
        {
            string[] itemStrings = items.Select(account => account.Title).ToArray();
            int currentIndex = 0;
            int currentScrollAmount = 0;
            int currentIndexPlusScroll = 0;
            int numberOfItemsToDisplay = Math.Min(items.Length, Console.WindowHeight - 3);
            char charInput;

            itemStrings = AddPaddingToOptions(itemStrings);

            Console.Clear();
            while (true)
            {
                DisplayMenu(title, itemStrings, currentIndex, currentScrollAmount, numberOfItemsToDisplay, true);
                charInput = Console.ReadKey(true).KeyChar;

                if (charInput == 'a')
                {
                    Console.Clear();
                    return items[currentIndexPlusScroll];
                }
                else if (charInput == 'z')
                {
                    Console.Clear();
                    return null;
                }
                else if (charInput == 'w' && currentIndexPlusScroll > 0)
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
                else if (charInput == 'd' && currentIndexPlusScroll < items.Length)
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
            }
        }

        private static void DisplayMenu(string title, string[] items, int currentIndex, int currentScrollAmount, int numberOfItemsToDisplay, bool displayNumberOfItems)
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
            if (displayNumberOfItems)
            {
                Utilities.ColorWrite(($"[w] UP [d] DOWN [a] SELECT [z] BACK/EXIT <{items.Length} ACCOUNTS>", false, ConsoleColor.Black, ConsoleColor.Blue));
                return;
            }
            Utilities.ColorWrite(("[w] UP [d] DOWN [a] SELECT [z] BACK/EXIT", false, ConsoleColor.Black, ConsoleColor.Blue));
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
