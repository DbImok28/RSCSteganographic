using System.Collections.Generic;

namespace RSCSteganographicMethod
{
    public static class AlphabetAnalyzer
    {
        public static List<char> AlphabetFromMessage(string message)
        {
            List<char> alphabet = new List<char>();
            foreach (var symbol in message)
            {
                if (!alphabet.Contains(symbol))
                {
                    alphabet.Add(symbol);
                }
            }
            return alphabet;
        }
        public static Dictionary<char, int> SymbolFrequency(List<char> alphabet, string data)
        {
            Dictionary<char, int> symbolFrequency = new Dictionary<char, int>();
            foreach (var symbol in alphabet)
            {
                symbolFrequency.Add(symbol, 0);
            }
            foreach (var symbol in data)
            {
                if (symbolFrequency.ContainsKey(symbol))
                {
                    symbolFrequency[symbol]++;
                }
            }
            return symbolFrequency;
        }
    }
}