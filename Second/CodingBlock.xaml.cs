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

namespace Cryptology.Second
{
    /// <summary>
    /// Логика взаимодействия для CodingBlock.xaml
    /// </summary>
    public partial class CodingBlock : UserControl
    {
        public CodingBlock()
        {
            InitializeComponent();
        }

        public void SetInputText(string text)
        {
            inputText.Document.Blocks.Clear();
            inputText.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        public string GetInputText()
        {
            return new TextRange(inputText.Document.ContentStart, inputText.Document.ContentEnd).Text.Trim();   
        }

        public void SetOutPutText(string text)
        {
            outputText.Document.Blocks.Clear();
            outputText.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        public string GetOutPutText()
        {
            return new TextRange(outputText.Document.ContentStart, outputText.Document.ContentEnd).Text.Trim();
        }

        private void buttonLift_Click(object sender, RoutedEventArgs e)
        {
            SetInputText(GetOutPutText());   
        }

        private void buttonCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GetOutPutText());
        }
    }
}
