using ReplacementSimilarCharactersSteganographicMethod.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RSCSteganographicMethod.Models
{
    // RSC - ReplacementSimilarCharacters SteganographicMethod

    public static class RSCSteganographicEncrypter
    {
        public static readonly string Name = "Steganographic method for replacing similar characters";

        public static string ToStringWithBits(char data, int symbolLength = 16)
        {
            byte[] bytes = BitConverter.GetBytes(data).Take(symbolLength / 8).ToArray();
            string bits = "";
            foreach (var dataByte in bytes)
            {
                bits += Convert.ToString(dataByte, 2).PadLeft(8, '0');
            }
            return bits;
        }

        public static char FromStringWithBits(string str, int symbolLength = 16)
        {
            byte[] bytes = new byte[symbolLength / 8];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(str.Substring(i * 8, 8), 2);
            }
            return BitConverter.ToChar(bytes);
        }

        public static string Encrypt(string rawData, string text, RSCAlphabet alphabet, int symbolLength = 16)
        {
            char[] buffer = new char[rawData.Length];
            int readIndex = 0;
            for (int textIndex = 0; textIndex < text.Length + 1; ++textIndex)
            {
                var symbol = '\0';
                if (textIndex < text.Length)
                {
                    symbol = text[textIndex];
                }

                var bits = ToStringWithBits(symbol, symbolLength);
                for (int bitIndex = 0; bitIndex < bits.Length;)
                {
                    var bit = bits[bitIndex];
                    var rawDataSymbol = rawData[readIndex];

                    if (alphabet.Contains(rawDataSymbol))
                    {
                        var (From, To) = alphabet.GetReplacementPair(rawDataSymbol);
                        if (bit == '0')
                        {
                            buffer[readIndex] = From;
                            ++bitIndex;
                        }
                        else
                        {
                            buffer[readIndex] = To;
                            ++bitIndex;
                        }
                    }
                    else
                    {
                        buffer[readIndex] = rawDataSymbol;
                    }
                    ++readIndex;
                    if (readIndex >= rawData.Length)
                    {
                        break;
                    }
                }
                if (readIndex >= rawData.Length)
                {
                    break;
                }
            }
            while (readIndex < rawData.Length)
            {
                buffer[readIndex] = rawData[readIndex];
                ++readIndex;
            }
            return string.Concat(buffer);
        }

        public static string Decrypt(string data, RSCAlphabet alphabet, int symbolLength = 16, string? encryptedtextAlphabet = null)
        {
            StringBuilder resutlSB = new StringBuilder();
            StringBuilder bitsSB = new StringBuilder();
            foreach (var symbol in data)
            {
                if (alphabet.Contains(symbol))
                {
                    var (From, _) = alphabet.GetReplacementPair(symbol);
                    if (symbol == From)
                    {
                        bitsSB.Append('0');
                    }
                    else
                    {
                        bitsSB.Append('1');
                    }
                }
                if (bitsSB.Length >= 16)
                {
                    var newSymbol = FromStringWithBits(bitsSB.ToString(), symbolLength);
                    if (newSymbol == '\0')
                    {
                        break;
                    }
                    resutlSB.Append(newSymbol);
                    bitsSB.Clear();
                }
            }
            return resutlSB.ToString();
        }

        public static string BenchmarkedEncrypt(out double time, string rawData, string text, RSCAlphabet alphabet, int symbolLength = 16)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = Encrypt(rawData, text, alphabet, symbolLength);
            time = stopWatch.Elapsed.TotalMilliseconds;
            return result;
        }

        public static string BenchmarkedDecrypt(out double time, string data, RSCAlphabet alphabet, int symbolLength = 16, string? encryptedtextAlphabet = null)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = Decrypt(data, alphabet, symbolLength, encryptedtextAlphabet);
            time = stopWatch.Elapsed.TotalMilliseconds;
            return result;
        }
    }
}
