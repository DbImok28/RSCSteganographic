using System.Windows;
using System.Windows.Controls;

namespace RSCSteganographicMethod.Infrastructure.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxWithNote.xaml
    /// </summary>
    public partial class TextBoxWithNote : UserControl
    {
        public TextBoxWithNote()
        {
            InitializeComponent();
            ApplyPasswordVisibility();
        }
        public string NoteText
        {
            get { return (string)GetValue(NoteTextProperty); }
            set { SetValue(NoteTextProperty, value); }
        }
        public static readonly DependencyProperty NoteTextProperty =
            DependencyProperty.Register("NoteText", typeof(string), typeof(TextBoxWithNote), new PropertyMetadata("note"));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWithNote), new PropertyMetadata(""));

        public bool ShowWithPassword
        {
            get { return (bool)GetValue(ShowWithPasswordProperty); }
            set
            {
                SetValue(ShowWithPasswordProperty, value);
            }
        }
        public static readonly DependencyProperty ShowWithPasswordProperty =
            DependencyProperty.Register("ShowWithPassword", typeof(bool), typeof(TextBoxWithNote), new PropertyMetadata(false, OnShowWithPasswordCallBack));
        public void ApplyPasswordVisibility()
        {
            InputPasswordBlock.Visibility = ShowWithPassword ? Visibility.Visible : Visibility.Hidden;
        }

        private static void OnShowWithPasswordCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBoxWithNote textBoxWithNote)
            {
                textBoxWithNote.ApplyPasswordVisibility();
            }
        }
        public void UpdateText()
        {
            InputTextBlock.Text = Text;
            UpdateTextVisibility();
        }
        public void UpdateTextVisibility()
        {
            if (Text == "")
            {
                NoteTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                NoteTextBlock.Visibility = Visibility.Hidden;
            }
        }
        private void InputTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = InputTextBlock.Text;
            UpdateTextVisibility();
        }

        private void InputPasswordBlock_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Text = InputPasswordBlock.Password;
            InputTextBlock.Text = Text;
            UpdateTextVisibility();
        }
    }
}
