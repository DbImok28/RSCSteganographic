using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace ReplacementSimilarCharactersSteganographicMethod.Models
{
    public class RSCAlphabet
    {
        public Dictionary<char, char> ReplaceDictionary { get; private set; } = new Dictionary<char, char>();

        public List<Alphabet> UsedAlphabets { get; set; } = new List<Alphabet>();

        public RSCAlphabet(Dictionary<char, char> replaceDictionary)
        {
            ReplaceDictionary = replaceDictionary;
        }

        public bool Contains(char c)
        {
            if (ReplaceDictionary.ContainsKey(c))
            {
                return true;
            }
            return ReplaceDictionary.ContainsValue(c);
        }

        public (char From, char To) GetReplacementPair(char c)
        {
            var finded = ReplaceDictionary.ToList().Find(x => x.Key == c || x.Value == c);
            return (finded.Key, finded.Value);
        }

        public bool Add(char from, char to)
        {
            if (Contains(from) || Contains(to))
            {
                return false;
            }
            ReplaceDictionary.Add(from, to);
            return true;
        }

        public void Remove(char key)
        {
            ReplaceDictionary.Remove(key);
        }

        public string FindAlphabetShortName(char c)
        {
            var finded = UsedAlphabets.Find(x => x.Contains(c));
            if (finded != null)
            {
                return finded.ShortName;
            }
            return "?";
        }

        #region StringOutput
        public List<KeyValuePair<string, string>> StringPairsWithLabel => GetStringPairsWithLabel();
        public List<KeyValuePair<string, string>> GetStringPairsWithLabel()
        {
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in ReplaceDictionary)
            {
                result.Add(new KeyValuePair<string, string>($"0: {item.Key}({FindAlphabetShortName(item.Key)})", $"1: {item.Value}({FindAlphabetShortName(item.Value)})"));
            }
            return result;
        }

        public List<string> StringPairs => GetStringPairs();

        public List<string> GetStringPairs()
        {
            var result = new List<string>();
            foreach (var item in ReplaceDictionary)
            {
                result.Add($"0: {item.Key}({FindAlphabetShortName(item.Key)}) -> 1: {item.Value}({FindAlphabetShortName(item.Value)})");
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in StringPairs)
            {
                sb.AppendLine(item);
            }
            return sb.ToString();
        }
        #endregion
    }
}
