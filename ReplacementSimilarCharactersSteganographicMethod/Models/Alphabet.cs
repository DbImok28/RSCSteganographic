using System.Linq;

namespace ReplacementSimilarCharactersSteganographicMethod.Models
{
    public class Alphabet
    {
        public readonly string Name = "None";
        public readonly string ShortName = "None";
        public char[] Symbols;

        public static Alphabet RusianAlphabet = new Alphabet("Rusian", "ru", "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя");
        public static Alphabet EnglishAlphabet = new Alphabet("English", "en", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");

        public Alphabet(string name, string shortName, char[] symbols)
        {
            Name = name;
            ShortName = shortName;
            Symbols = symbols;
        }
        public Alphabet(string name, string shortName, string symbols) : this(name, shortName, symbols.ToCharArray()) { }

        public bool Contains(char c)
        {
            //return Symbols.Contains(c);
            foreach (var symbol in Symbols)
            {
                if (symbol == c)
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
