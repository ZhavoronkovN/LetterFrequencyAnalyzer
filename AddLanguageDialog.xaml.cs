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
using System.Windows.Shapes;

namespace Cryptology
{
    /// <summary>
    /// Логика взаимодействия для AddLanguageDialog.xaml
    /// </summary>
    public partial class AddLanguageDialog : Window
    {
        Action<string,string> OnExit;
        public AddLanguageDialog(Action<string,string> onExit)
        {
            InitializeComponent();
            //Method will be called when user clicks Confirm
            OnExit = onExit;
        }

        private void Confrim(object sender, RoutedEventArgs e)
        {
            OnExit(nameInput.Text, alphabetInput.Text);
            this.Close();
        }
    }
}
