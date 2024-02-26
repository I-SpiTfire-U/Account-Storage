namespace Account_Storage.Source
{
    internal class Menu
    {
        internal object[] Items;
        private int _Index;
        private int _Page;
        private readonly int _ScreenHeight = Console.WindowHeight - 3;

        internal Menu(params object[] items)
        {
            Items = items;
        }

        private void DisplayMenu(string escapePrompt)
        {
            for (int i = 0; i < _ScreenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine(Environment.NewLine);
                Console.SetCursorPosition(0, i);
                Console.WriteLine(i == _Index ? $"> {Items[i + (_Page * _ScreenHeight)]}" : $"  {Items[i + (_Page * _ScreenHeight)]}");

                if (i + (_Page * _ScreenHeight) >= Items.Length - 1)
                {
                    break;
                }
            }
            Console.SetCursorPosition(0, _ScreenHeight + 1);
            Utilities.ColorWrite(ConsoleColor.Black, ConsoleColor.DarkCyan, $"[▼] Down [▲] Up [ENTER] Choose [ESCAPE] {escapePrompt}");
        }

        internal int StartMenuLoop(string escapePrompt = "Exit")
        {
            Console.Clear();
            while (true)
            {
                DisplayMenu(escapePrompt);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (_Index == 0 && _Page == 0)
                        {
                            continue;
                        }
                        _Index--;

                        if (_Index == -1)
                        {
                            _Index = _ScreenHeight - 1;
                            _Page--;
                            Console.Clear();
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (_Index + (_Page * _ScreenHeight) == Items.Length - 1)
                        {
                            continue;
                        }
                        _Index++;

                        if (_Index == _ScreenHeight && _Index < Items.Length - 1)
                        {
                            _Index = 0;
                            _Page++;
                            Console.Clear();
                        }
                        break;
                    case ConsoleKey.Enter:
                        return _Index + (_Page * _ScreenHeight);
                    case ConsoleKey.Escape:
                        return -1;
                }
            }
        }
    }
}
