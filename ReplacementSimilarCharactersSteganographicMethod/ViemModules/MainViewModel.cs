using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using ReplacementSimilarCharactersSteganographicMethod.Models;
using RSCSteganographicMethod.Infrastructure.Commands;
using RSCSteganographicMethod.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace RSCSteganographicMethod.ViemModules
{
    public class MainViewModel : BaseViewModel
    {
        #region Title
        private string _Title = "Steganographic method for replacing similar characters";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region OperationType
        private List<string> _OperationType;
        public List<string> OperationType
        {
            get => _OperationType;
            set => Set(ref _OperationType, value);
        }
        #endregion
        #region SelectedOperationTypeIndex
        private int _SelectedOperationTypeIndex = 0;
        public int SelectedOperationTypeIndex
        {
            get => _SelectedOperationTypeIndex;
            set => Set(ref _SelectedOperationTypeIndex, value);
        }
        #endregion

        #region Files
        #region SourceEncryptFile
        private string _SourceEncryptFile = "";
        public string SourceEncryptFile
        {
            get => _SourceEncryptFile;
            set
            {
                Set(ref _SourceEncryptFile, value);
                OnPropertyChanged(nameof(FileCappacityStr));
            }
        }
        #endregion
        #region ResultEncryptFile
        private string _ResultEncryptFile = "";
        public string ResultEncryptFile
        {
            get => _ResultEncryptFile;
            set => Set(ref _ResultEncryptFile, value);
        }
        #endregion
        #region SourceDecryptFile
        private string _SourceDecryptFile;
        public string SourceDecryptFile
        {
            get => _SourceDecryptFile;
            set => Set(ref _SourceDecryptFile, value);
        }
        #endregion
        #endregion

        #region ReplacementAlphabet
        private RSCAlphabet _ReplacementAlphabet;
        public RSCAlphabet ReplacementAlphabet
        {
            get => _ReplacementAlphabet;
            set => Set(ref _ReplacementAlphabet, value);
        }
        #endregion

        #region Message
        private string _Message = "";
        public string Message
        {
            get => _Message;
            set
            {
                Set(ref _Message, value);
                OnPropertyChanged(nameof(FileCappacityStr));
            }
        }
        #endregion
        #region DecryptedMessage
        private string _DecryptedMessage;
        public string DecryptedMessage
        {
            get => _DecryptedMessage;
            set => Set(ref _DecryptedMessage, value);
        }
        #endregion

        #region BitsInMessage
        private int _BitsInMessage = 16;
        public int BitsInMessage
        {
            get => _BitsInMessage;
            set
            {
                Set(ref _BitsInMessage, value);
                OnPropertyChanged(nameof(FileCappacityStr));
            }
        }
        #endregion

        #region EncryptionTime
        private double _EncryptionTime;
        public double EncryptionTime
        {
            get => _EncryptionTime;
            set => Set(ref _EncryptionTime, value);
        }
        #endregion
        #region DecryptionTime
        private double _DecryptionTime;
        public double DecryptionTime
        {
            get => _DecryptionTime;
            set => Set(ref _DecryptionTime, value);
        }
        #endregion

        #region FileCappacity
        public long FileCappacity
        {
            get
            {
                if (!string.IsNullOrEmpty(SourceEncryptFile) && File.Exists(SourceEncryptFile))
                {
                    return RSCSteganographicAnalyzer.Capacity(File.ReadAllText(SourceEncryptFile), ReplacementAlphabet);
                }
                return 0;
            }
        }
        #endregion
        #region FileCappacityStr
        public string FileCappacityStr
        {
            get
            {
                if (!string.IsNullOrEmpty(SourceEncryptFile) && File.Exists(SourceEncryptFile))
                {
                    var capacity = RSCSteganographicAnalyzer.Capacity(File.ReadAllText(SourceEncryptFile), ReplacementAlphabet);
                    return $"Занято {(Message.Length + 1) * BitsInMessage} из {capacity} бит";
                }
                return "0";
            }
        }
        #endregion

        #region Encrypt
        public ICommand EncryptCommand { get; }
        private void OnEncryptCommandExecuted(object? par) => Encrypt();
        #endregion
        #region RemoveReplacement
        public ICommand RemoveReplacementCommand { get; }
        private void OnRemoveReplacementCommandExecuted(object? par) => RemoveReplacement(par != null ? (string)par : "");
        #endregion
        #region AddReplacement
        public ICommand AddReplacementCommand { get; }
        private void OnAddReplacementCommandExecuted(object? par) => AddReplacement((object[])par);
        #endregion
        #region Decrypt
        public ICommand DecryptCommand { get; }
        private void OnDecryptCommandExecuted(object? par) => Decrypt();
        #endregion

        public MainViewModel()
        {
            EncryptCommand = new LambdaCommand(OnEncryptCommandExecuted);
            DecryptCommand = new LambdaCommand(OnDecryptCommandExecuted);
            RemoveReplacementCommand = new LambdaCommand(OnRemoveReplacementCommandExecuted);
            AddReplacementCommand = new LambdaCommand(OnAddReplacementCommandExecuted);

            _OperationType = new List<string>()
            {
                "Encrypt",
                "Decrypt"
            };
            _SelectedOperationTypeIndex = 0;

            _ReplacementAlphabet = new RSCAlphabet(new Dictionary<char, char>()
            {
                //рус  анг
                { 'a', 'а' },
                { 'A', 'А' },

                { 'c', 'с' },
                { 'C', 'С' },

                { 'O', 'О' },
                { 'o', 'о' },

                { 'E', 'Е' },
                { 'e', 'е' },

                { 'P', 'Р' },
                { 'p', 'р' },

                { 'X', 'Х' },
                { 'x', 'х' },

                { 'B', 'В' },
                { 'K', 'К' },
                { 'M', 'М' },
                { 'H', 'Н' },
                { 'T', 'Т' },

                { 'y', 'у' },
            });
            _ReplacementAlphabet.UsedAlphabets.Add(Alphabet.RusianAlphabet);
            _ReplacementAlphabet.UsedAlphabets.Add(Alphabet.EnglishAlphabet);
        }

        public bool CheckFile(string path, string msgToPath, string msgToNotExist)
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show($"Введите путь к {msgToPath} файлу", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!File.Exists(path))
            {
                MessageBox.Show($"{msgToNotExist} файл не существует", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        public bool CheckFile(string path, string msgToPath)
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show($"Введите путь к {msgToPath} файлу", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool CheckEncryptInput()
        {
            CheckFile(SourceEncryptFile, "исходному", "Исходный");
            CheckFile(ResultEncryptFile, "результирующему");
            return true;
        }
        public bool CheckDecryptInput()
        {
            CheckFile(SourceDecryptFile, "исходному", "Исходный");
            return true;
        }

        void RemoveReplacement(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ReplacementAlphabet.Remove(key[3]);
                OnPropertyChanged(nameof(ReplacementAlphabet));
            }
        }

        void AddReplacement(object[] pair)
        {
            if (pair.Length == 2 && !string.IsNullOrEmpty((string)pair[0]) && !string.IsNullOrEmpty((string)pair[1]))
            {
                ReplacementAlphabet.Add(((string)pair[0])[0], ((string)pair[1])[0]);
                OnPropertyChanged(nameof(ReplacementAlphabet));
            }
        }

        public void Encrypt()
        {
            if (!CheckEncryptInput()) return;
            string sourceText = File.ReadAllText(SourceEncryptFile);
            string resultText = RSCSteganographicEncrypter.BenchmarkedEncrypt(out double encryptionTime, sourceText, Message, ReplacementAlphabet, BitsInMessage);
            EncryptionTime = encryptionTime;
            if (Path.GetExtension(ResultEncryptFile) == ".pdf")
            {
                //PdfReader reader = new PdfReader(ResultEncryptFile);
                //string text = string.Empty;

                //reader.
                //for (int page = 1; page <= reader.NumberOfPages; page++)
                //{
                //    text += PdfTextExtractor.GetTextFromPage(reader, page);
                //}
                //reader.Close();
                //return text;
            }
            else
            {
                File.WriteAllText(ResultEncryptFile, resultText);
            }
        }

        public void Decrypt()
        {
            if (!CheckDecryptInput()) return;
            string sourceText = File.ReadAllText(SourceDecryptFile);
            DecryptedMessage = RSCSteganographicEncrypter.BenchmarkedDecrypt(out double decryptionTime, sourceText, ReplacementAlphabet, BitsInMessage);
            DecryptionTime = decryptionTime;
        }
    }
}
