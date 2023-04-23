using ReplacementSimilarCharactersSteganographicMethod.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace RSCSteganographicMethod.Models
{
    // RSC - ReplacementSimilarCharacters SteganographicMethod

    public static class RSCSteganographicEncrypter
    {
        public static readonly string Name = "Steganographic method for replacing similar characters";

        public static string ToStringWithBits(char data)
        {
            return Convert.ToString((byte)data, 2).PadLeft(8, '0');
        }

        public static char FromStringWithBits(string str)
        {
            return (char)Convert.ToByte(str.Substring(0, 8), 2);
        }

        public static string Encrypt(string rawData, string text, RSCAlphabet alphabet)
        {
            char[] buffer = new char[rawData.Length];
            for (int textIndex = 0; textIndex < text.Length + 1; ++textIndex)
            {
                var symbol = '\0';
                if (textIndex < text.Length)
                {
                    symbol = text[textIndex];
                }

                var bits = ToStringWithBits(symbol);
                foreach (var bit in bits)
                {
                    for (int readIndex = 0; readIndex < rawData.Length; readIndex++)
                    {
                        var rawDataSymbol = rawData[readIndex];
                        if (alphabet.Contains(rawDataSymbol))
                        {
                            var (From, To) = alphabet.GetReplacementPair(rawDataSymbol);
                            if (bit == '0')
                            {
                                buffer[readIndex] = From;
                            }
                            else
                            {
                                buffer[readIndex] = To;
                            }
                        }
                        else
                        {
                            buffer[readIndex] = rawDataSymbol;
                        }
                    }
                }
            }
            return string.Concat(buffer);
        }

        public static string Decrypt(string data, RSCAlphabet alphabet, string? encryptedtextAlphabet = null)
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
                if (bitsSB.Length >= 8)
                {
                    var newSymbol = FromStringWithBits(bitsSB.ToString());
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

        public static string BenchmarkedEncrypt(out double time, string rawData, string text, RSCAlphabet alphabet)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = Encrypt(rawData, text, alphabet);
            time = stopWatch.Elapsed.TotalMilliseconds;
            return result;
        }

        public static string BenchmarkedDecrypt(out double time, string data, RSCAlphabet alphabet, string? encryptedtextAlphabet = null)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            var result = Decrypt(data, alphabet, encryptedtextAlphabet);
            time = stopWatch.Elapsed.TotalMilliseconds;
            return result;
        }
    }
}
