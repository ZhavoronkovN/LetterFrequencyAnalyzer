using Cryptology.Third;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Cryptology
{
    /// <summary>
    /// Логика взаимодействия для ThirdTab.xaml
    /// </summary>
    public partial class ThirdTab : UserControl
    {
        LogicThird _logic = new LogicThird();
        public ThirdTab()
        {
            InitializeComponent();
            codingBlock.buttonDecode.Click += ButtonDecode_Click;
            codingBlock.buttonEncode.Click += ButtonEncode_Click;
            toolBlock.buttonOpenFile.Click += ButtonOpenFile_Click;
            toolBlock.buttonSaveToFile.Click += ButtonSaveToFile_Click;
            toolBlock.buttonImport.Click += ButtonImport_Click;
            toolBlock.buttonExport.Click += ButtonExport_Click;
            keyBlock.buttonRandom.Click += ButtonRandom_Click;
        }

        private void ButtonRandom_Click(object sender, RoutedEventArgs e)
        {
            keyBlock.inputKey.Text = _logic.Random(languageBlock.CurrentLanguage, codingBlock.GetInputText().Length);
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".key";
            openFileDialog.CheckFileExists = false;
            if (openFileDialog.ShowDialog() == true)
            {
                if (_logic.WriteKey(openFileDialog.FileName,keyBlock.inputKey.Text))
                {
                    MessageBox.Show("Key saved");
                }
                else
                {
                    MessageBox.Show("Error writing to file!");
                }
            }
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".key";
            if (openFileDialog.ShowDialog() == true)
            {
                keyBlock.inputKey.Text = _logic.ReadKey(openFileDialog.FileName);
            }
        }

        private void ButtonSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.CheckFileExists = false;
            if (openFileDialog.ShowDialog() == true)
            {
                if (_logic.WriteFile(codingBlock.GetOutPutText(), openFileDialog.FileName))
                {
                    MessageBox.Show("Successfully saved to file");
                }
                else
                {
                    MessageBox.Show("Error writing to file!");
                }
            }
        }

        private void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            if (openFileDialog.ShowDialog() == true)
            {
                string text = _logic.ReadFile(openFileDialog.FileName);
                if (!String.IsNullOrEmpty(text))
                {
                    codingBlock.SetInputText(text);
                }
                else
                {
                    MessageBox.Show("Cannot read file!");
                }
            }
        }

        private void ButtonEncode_Click(object sender, RoutedEventArgs e)
        {
            if (!CanWork(true))
            {
                MessageBox.Show("Please check key length!");
                return;
            }
            if (!CheckKey())
            {
                MessageBox.Show("Key has characters which are not presented in current language!");
                return;
            }
            if (!CheckText())
            {
                MessageBox.Show("Text has characters which are not presented in current language!");
                return;
            }
            string result = _logic.Encode(codingBlock.GetInputText(), keyBlock.inputKey.Text, languageBlock.CurrentLanguage);
            if (String.IsNullOrEmpty(result))
            {
                MessageBox.Show("Cannot process text");
            }
            else
            {
                codingBlock.SetOutPutText(result);
            }
        }

        private void ButtonDecode_Click(object sender, RoutedEventArgs e)
        {
            if (!CanWork(false))
            {
                MessageBox.Show("Please check key length!");
                return;
            }
            if (!CheckKey())
            {
                MessageBox.Show("Key has characters which are not presented in current language!");
                return;
            }
            string result = _logic.Decode(codingBlock.GetInputText(), keyBlock.inputKey.Text, languageBlock.CurrentLanguage);
            if (String.IsNullOrEmpty(result))
            {
                MessageBox.Show("Cannot process text");
            }
            else
            {
                codingBlock.SetOutPutText(result);
            }
        }

        private bool CanWork(bool encodeMode)
        {
            return encodeMode ? keyBlock.inputKey.Text.Length >= codingBlock.GetInputText().Length : keyBlock.inputKey.Text.Length >= codingBlock.GetInputText().Split(' ').Length;
        }

        private bool CheckKey()
        {
            bool result = true;
            keyBlock.inputKey.Text.ToList().ForEach((ch) => result = result && Common.Languages[languageBlock.CurrentLanguage].Contains(ch));
            return result;
        }

        private bool CheckText()
        {
            bool result = true;
            codingBlock.GetInputText().ToList().ForEach((ch) => result = result && Common.Languages[languageBlock.CurrentLanguage].Contains(ch));
            return result;
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vernam cipher\nVersion 6.6.6 omega\nAll rights reserved");
        }
    }
}
