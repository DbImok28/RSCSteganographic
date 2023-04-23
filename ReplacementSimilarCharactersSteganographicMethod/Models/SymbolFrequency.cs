namespace RSCSteganographicMethod.Models
{
    public struct SymbolFrequency
    {
        public char Symbol { get; set; }
        public float Frequency { get; set; }

        public SymbolFrequency(char symbol, float frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
        }
    }
}
