using System.Linq;

namespace ReplacementSimilarCharactersSteganographicMethod.Models
{
    public static class RSCSteganographicAnalyzer
    {
        public static long Capacity(string text, RSCAlphabet alphabet)
        {
            return text.Count(x => alphabet.Contains(x));
        }
    }
}
