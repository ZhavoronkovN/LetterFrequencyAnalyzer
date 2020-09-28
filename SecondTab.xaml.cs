using Cryptology.Second;
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
    
    public partial class SecondTab : UserControl
    {
        LogicSecond _logic;
        public SecondTab()
        {
            _logic = new LogicSecond();
            InitializeComponent();
            codingBlock.buttonDecode.Click += ButtonDecode_Click;
            codingBlock.buttonEncode.Click += ButtonEncode_Click;
            toolBlock.buttonOpenFile.Click += ButtonOpenFile_Click;
            toolBlock.buttonSaveToFile.Click += ButtonSaveToFile_Click;
            toolBlock.buttonImport.Click += ButtonImport_Click;
            toolBlock.buttonExport.Click += ButtonExport_Click;
            ruleBlock.buttonRandom.Click += ButtonRandom_Click;
            languageBlock_LanguageChanged(this, languageBlock.CurrentLanguage);
        }

        private void ButtonRandom_Click(object sender, RoutedEventArgs e)
        {
            _logic.Random(languageBlock.CurrentLanguage);
            ruleBlock.outputLang.Text = string.Join("",_logic.Substitutions[languageBlock.CurrentLanguage]);
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".rule";
            openFileDialog.CheckFileExists = false;
            if (openFileDialog.ShowDialog() == true)
            {
                if (_logic.WriteRules(openFileDialog.FileName))
                {
                    MessageBox.Show("Rules saved");
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
            openFileDialog.DefaultExt = ".rule";
            if (openFileDialog.ShowDialog() == true)
            {
                if (_logic.ReadRules(openFileDialog.FileName))
                {
                    MessageBox.Show("Rules updated");
                    ruleBlock.outputLang.Text = string.Join("", _logic.Substitutions[languageBlock.CurrentLanguage]);
                }
                else
                {
                    MessageBox.Show("Cannot read rules!");
                }
            }
        }

        private void ButtonSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.CheckFileExists = false;
            if (openFileDialog.ShowDialog() == true)
            {
                if (_logic.WriteFile(codingBlock.GetOutPutText(),openFileDialog.FileName))
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
            if (CanWork())
            {
                string result = _logic.Encode(ruleBlock.inputLang.Text, ruleBlock.outputLang.Text, codingBlock.GetInputText());
                if (String.IsNullOrEmpty(result))
                {
                    MessageBox.Show("Cannot process text");
                }
                else
                {
                    codingBlock.SetOutPutText(result);
                }
            }
            else
            {
                MessageBox.Show("Please check rules!");
            }
        }

        private void ButtonDecode_Click(object sender, RoutedEventArgs e)
        {
            if (CanWork())
            {
                string result = _logic.Encode(ruleBlock.outputLang.Text, ruleBlock.inputLang.Text, codingBlock.GetInputText());
                if (String.IsNullOrEmpty(result))
                {
                    MessageBox.Show("Cannot process text");
                }
                else
                {
                    codingBlock.SetOutPutText(result);
                }
            }
            else
            {
                MessageBox.Show("Please check rules!");
            }
        }
        
        private bool CanWork()
        {
            return ruleBlock.inputLang.Text.Length == ruleBlock.outputLang.Text.Length && ruleBlock.inputLang.Text.Length == Common.Languages[languageBlock.CurrentLanguage].Length;
        }

        private void languageBlock_LanguageChanged(object sender, string e)
        {
            ruleBlock.inputLang.Text = string.Join("", Common.Languages[languageBlock.CurrentLanguage]);
            ruleBlock.outputLang.Text = string.Join("", _logic.Substitutions[languageBlock.CurrentLanguage]);
        }

        private void languageBlock_LanguageAdded(object sender, string e)
        {
            _logic.AddLanguage(e);
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Substitution cipher\nVersion 0.0.0.-1 beta\nAll rights reserved");
        }
    }
}
