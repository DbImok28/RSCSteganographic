using ReplacementSimilarCharactersSteganographicMethod.Models;
using RSCSteganographicMethod.Infrastructure.Commands;
using RSCSteganographicMethod.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
            set => Set(ref _SourceEncryptFile, value);
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

        #region ResultDecryptFile
        private string _ResultDecryptFile;
        public string ResultDecryptFile
        {
            get => _ResultDecryptFile;
            set => Set(ref _ResultDecryptFile, value);
        }
        #endregion
        #endregion
        //#region FrequencyAllocationOfOriginalMessage
        //private ObservableCollection<SymbolFrequency> _FrequencyAllocOfOriginalMessage = new();
        //public ObservableCollection<SymbolFrequency> FrequencyAllocOfOriginalMessage
        //{
        //    get => _FrequencyAllocOfOriginalMessage;
        //    set => Set(ref _FrequencyAllocOfOriginalMessage, value);
        //}
        //#endregion
        //#region FrequencyAllocationOfOriginalMessage
        //private ObservableCollection<SymbolFrequency> _FrequencyAllocOfRecivedMessage = new();
        //public ObservableCollection<SymbolFrequency> FrequencyAllocOfRecivedMessage
        //{
        //    get => _FrequencyAllocOfRecivedMessage;
        //    set => Set(ref _FrequencyAllocOfRecivedMessage, value);
        //}
        //#endregion

        #region ReplacementAlphabet
        private RSCAlphabet _ReplacementAlphabet;
        public RSCAlphabet ReplacementAlphabet
        {
            get => _ReplacementAlphabet;
            set => Set(ref _ReplacementAlphabet, value);
        }
        #endregion

        #region Message
        private string _Message;
        public string Message
        {
            get => _Message;
            set => Set(ref _Message, value);
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

        #region Encrypt
        public ICommand EncryptCommand { get; }
        private void OnEncryptCommandExecuted(object? par) => Encrypt();
        #endregion
        #region Decrypt
        public ICommand DecryptCommand { get; }
        private void OnDecryptCommandExecuted(object? par) => Decrypt();
        #endregion

        public MainViewModel()
        {
            EncryptCommand = new LambdaCommand(OnEncryptCommandExecuted);
            DecryptCommand = new LambdaCommand(OnDecryptCommandExecuted);

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

        //public void UpdateFrequency(string sourceText, string resultText)
        //{
        //    FrequencyAllocOfOriginalMessage = new ObservableCollection<SymbolFrequency>(
        //       AlphabetAnalyzer.SymbolFrequency(Alphabet.ToList(), sourceText)
        //       .Select(x => new SymbolFrequency(x.Key, x.Value / (float)sourceText.Length * 100.0f)));

        //    FrequencyAllocOfRecivedMessage = new ObservableCollection<SymbolFrequency>(
        //        AlphabetAnalyzer.SymbolFrequency(Alphabet.ToList(), resultText)
        //        .Select(x => new SymbolFrequency(x.Key, x.Value / (float)resultText.Length * 100.0f)));

        //    OnPropertyChanged(nameof(SelectedOperationTypeIndex));
        //}

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
            CheckFile(ResultDecryptFile, "результирующему");
            return true;
        }

        public void Encrypt()
        {
            if (!CheckEncryptInput()) return;
            string sourceText = File.ReadAllText(SourceEncryptFile);
            string resultText = RSCSteganographicEncrypter.BenchmarkedEncrypt(out double encryptionTime, sourceText, Message, ReplacementAlphabet);
            EncryptionTime = encryptionTime;
            File.WriteAllText(ResultEncryptFile, resultText);
            //UpdateFrequency(sourceText, resultText);
        }

        public void Decrypt()
        {
            if (!CheckDecryptInput()) return;
            string sourceText = File.ReadAllText(SourceDecryptFile);
            string resultText = RSCSteganographicEncrypter.BenchmarkedDecrypt(out double decryptionTime, sourceText, ReplacementAlphabet);
            DecryptionTime = decryptionTime;
            File.WriteAllText(ResultDecryptFile, resultText);
            //UpdateFrequency(sourceText, resultText);
        }
    }
}
