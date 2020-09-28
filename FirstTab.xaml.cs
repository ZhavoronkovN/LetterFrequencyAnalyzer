using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Cryptology.First;

namespace Cryptology
{
    public partial class FirstTab : UserControl
    {
        First.LogicFirst _logic;
        public ObservableCollection<First.DataElement> DataElements { get; set; }

        private string CurrentLanguage { get { return languageBlock.CurrentLanguage; } }
        public FirstTab()
        {
            DataElements = new ObservableCollection<First.DataElement>();
            InitializeComponent();
            _logic = new First.LogicFirst();

            languageBlock.LanguageAdded += LanguageBlock_LanguageAdded;
            LanguageChanged(this, languageBlock.CurrentLanguage);

            toolBlock.buttonUpdate.Click += UpdateWithFile;
            toolBlock.buttonClear.Click += ClearData;
            toolBlock.buttonImport.Click += ImportStat;
            toolBlock.buttonExport.Click += ExportStat;
        }

        private void ExportStat(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.CheckFileExists = false;
            if (openFileDialog.ShowDialog() == true)
            {
                if (_logic.ExportStat(CurrentLanguage, openFileDialog.FileName))
                {
                    MessageBox.Show("Export finished successfully");
                }
                else
                {
                    MessageBox.Show("Error while exporting");
                }
            }
        }

        private void ImportStat(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            if (openFileDialog.ShowDialog() == true)
            {
                if (_logic.ImportStat(CurrentLanguage, openFileDialog.FileName))
                {
                    LanguageChanged(this, CurrentLanguage);
                }
                else
                {
                    MessageBox.Show("Error while importing");
                }
            }
        }

        private void ClearData(object sender, RoutedEventArgs e)
        {
            _logic.ClearCalculated(CurrentLanguage);
            LanguageChanged(this, CurrentLanguage);
        }

        private void UpdateWithFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            if (openFileDialog.ShowDialog() == true)
            {
                if (_logic.UpdateWithFile(CurrentLanguage, openFileDialog.FileName))
                {
                    LanguageChanged(this, CurrentLanguage);
                }
                else
                {
                    MessageBox.Show("Error in updating");
                }
            }
        }

        private void LanguageBlock_LanguageAdded(object sender, string e)
        {
            _logic.AddLanguage(e);
        }

        private void LanguageChanged(object sender, string Language)
        {
            DataElements.Clear();
            _logic.CalculatedLanguages[Language].ForEach((First.DataElement x) => DataElements.Add(x));
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Letter Frequency Analyzer\nVersion 0.0.0.1 beta\nAll rights reserved");
        }
    }
}
