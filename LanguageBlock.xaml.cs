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

namespace Cryptology
{
    /// <summary>
    /// Логика взаимодействия для LanguageBlock.xaml
    /// </summary>
    public partial class LanguageBlock : UserControl
    {
        public event EventHandler<string> LanguageChanged;
        public event EventHandler<string> LanguageAdded;

        public string CurrentLanguage;

        public LanguageBlock()
        {
            InitializeComponent();

            //draw button for every language in Logic.Languages
            foreach(string language in Common.Languages.Keys)
            {
                RadioButton item = new RadioButton();
                item.Content = language;
                item.FontSize = 16;
                item.IsChecked = stack.Children.Count == 0;
                if (item.IsChecked.Value)
                {
                    CurrentLanguage = language;
                }
                item.Click += ChangeLanguage;
                stack.Children.Add(item);
            }
        }

        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            foreach(var element in stack.Children)
            {
                var button = element as RadioButton;
                if (button.IsChecked.Value)
                {
                    CurrentLanguage = (string)button.Content;
                    LanguageChanged?.Invoke(this, (string)button.Content);
                }
            }
        }

        private void AddLanguage(object sender, RoutedEventArgs e)
        {
            new AddLanguageDialog((string name,string alphabet)=> {
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(alphabet) && alphabet.Split(',').Length>0)
                {
                    //getting char[] from string and adding to languages
                    List<char> charList = new List<char>();
                    alphabet.Split(',').ToList().ForEach((string obj) => charList.Add(obj.First()));
                    Common.Languages.Add(name, charList.ToArray());
                    LanguageAdded?.Invoke(this,name);

                    //adding radio button to GUI
                    RadioButton item = new RadioButton();
                    item.Content = name;
                    item.FontSize = 16;
                    item.Click += ChangeLanguage;
                    stack.Children.Add(item);
                }
                else
                {
                    MessageBox.Show("Cant add language. Please provide correct name and alphabet");
                }
            }).Show();
        }
    }
}
