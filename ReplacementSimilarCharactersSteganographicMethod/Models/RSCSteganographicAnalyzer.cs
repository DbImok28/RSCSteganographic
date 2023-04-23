using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplacementSimilarCharactersSteganographicMethod.Models
{
    public class RSCSteganographicAnalyzer
    {
        private string Text { get; set; }
        private RSCAlphabet Alphabet { get; set; }

        public RSCSteganographicAnalyzer(string text, RSCAlphabet alphabet)
        {
            Text = text;
            Alphabet = alphabet;
        }

        public long Capacity()
        {
            return Text.Count(x => Alphabet.ReplaceDictionary.ContainsKey(x));
        }
    }
}
