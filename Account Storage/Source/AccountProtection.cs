using System.Text;

namespace Account_Storage.Source
{
    internal static class AccountProtection
    {
        private static readonly Dictionary<string, char> CharacterChunks = new()
        {
            {"A1b$C@d", 'a'},  {"E2f#G^h", 'b'},  {"I3j&K*@", 'c'},  {"L4m()P+", 'd'},  {"Q5n_R=Y", 'e'},
            {"S6o{T}l", 'f'},  {"U7p;V~'", 'g'},  {"W8q/x$<", 'h'},  {"(Y9r>z`", 'i'},  {"A0s-!#3", 'j'},
            {"B1t$%&s", 'k'},  {"C2u^*(;", 'l'},  {"D3v)^_[", 'm'},  {"E4w]{}:", 'n'},  {"F5x;'7<", 'o'},
            {"G6y>,./", 'p'},  {"H7z:`^_", 'q'},  {"I8a0!@#", 'r'},  {"J9Bb$%(", 's'},  {"K0/c^*+", 't'},
            {"L1d)_=z", 'u'},  {"M2e'{;:", 'v'},  {"N3f}'{<", 'w'},  {"O42g>,/", 'x'},  {"P5h?`~_", 'y'},
            {"Q6i!@#w", 'z'},  {"R7j+$%&", 'A'},  {"S8}k^*(", 'B'},  {"T9l0)_=", 'C'},  {"U0m{+;:", 'D'},
            {"V1n}'<o", 'E'},  {"W2o>,/=", 'F'},  {"X3pT`~_", 'G'},  {"Y4q#!@#", 'H'},  {"Z-5r$%&", 'I'},
            {"A6s^*(X", 'J'},  {"B7t)_=@", 'K'},  {"CC8u{;:", 'L'},  {"D9v}'/<", 'M'},  {"E0@w>,/", 'N'},
            {"F1x`~_V", 'O'},  {"G2y!@##", 'P'},  {"H3Vz$%&", 'Q'},  {"I4a*^*(", 'R'},  {"J5!b)_=", 'S'},
            {"K6c{;:3", 'T'},  {"L7d}'<I", 'U'},  {"M48e>,/", 'V'},  {"N9f%`~_", 'W'},  {"O0g)!@#", 'X'},
            {"P1h$%&1", 'Y'},  {"Q2iK^*(", 'Z'},  {"R39j)_=", '0'},  {"S4k{v;:", '1'},  {"T5l}'(<", '2'},
            {"U6m>,/0", '3'},  {"V7n-`~_", '4'},  {"W8o!0@#", '5'},  {"X9pe$%&", '6'},  {"Y0q^0*(", '7'},
            {"Z1r)_=0", '8'},  {"A21s{;:", '9'},  {"B3t}'/<", '!'},  {"C4u>0,/", '@'},  {"D5v`A~_", '#'},
            {"E6w!@#Q", '$'},  {"F7x3$%&", '%'},  {"G8/y^*(", '^'},  {"H9z=)_=", '&'},  {"I0a{;D:", '*'},
            {"J1b}'<N", '('},  {"K27c>,/", ')'},  {"L3d?`~_", '_'},  {"M4e+!@#", '+'},  {"o2[]_;A", '-'},
            {"N5f$9%&", '='},  {"4O6g^*(", '['},  {"P7h9)_=", ']'},  {"Q!8i{;:", ';'},  {"R9j}'^<", ':'},
            {"S0k>,)/", '\''}, {"YT1l`~_", '"'},  {"U2m!S@#", '\\'}, {"V3n8$%&", '/'},  {"W4o^*~(", '?'},
            {"X5#p)_=", ','},  {"Y96q{;:", '.'},  {"Z7r}')<", '~'},  {"apo}@#?", '|'},  {")3&a2;N", ' '}
        };

        internal static string Encrypt(string value)
        {
            StringBuilder stringBuilder = new();
            foreach (char character in value)
            {
                foreach (KeyValuePair<string, char> kvp in CharacterChunks)
                {
                    if (kvp.Value == character)
                    {
                        stringBuilder.Append(kvp.Key);
                    }
                }
            }
            return stringBuilder.ToString();
        }

        internal static string Decrypt(string value)
        {
            StringBuilder sb = new();
            string[] result = SplitStringIntoChunks(value, 7);

            foreach (string s in result)
            {
                sb.Append(CharacterChunks[s]);
            }

            return sb.ToString();
        }

        private static string[] SplitStringIntoChunks(string value, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentException("Chunk size must be greater than 0", nameof(chunkSize));
            }

            int totalChunks = (int)Math.Ceiling((double)value.Length / chunkSize);
            string[] chunks = new string[totalChunks];

            for (int i = 0; i < totalChunks; i++)
            {
                int startIndex = i * chunkSize;
                int length = Math.Min(chunkSize, value.Length - startIndex);
                chunks[i] = value.Substring(startIndex, length);
            }

            return chunks;
        }
    }
}