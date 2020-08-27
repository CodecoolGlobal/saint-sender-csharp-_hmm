using SaintSender.DesktopUI.ViewModels;
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

namespace SaintSender.DesktopUI.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        MainViewModel _vm;
        public LoginWindow()
        {
            InitializeComponent();
            _vm = MainViewModel.GetInstance();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.handleLogIn(emailInput.Text, passwordInput.Password))
            {
                //_vm.handleLogIn(emailInput.Text, passwordInput.Password);
                this.Close();
            }
            else 
            {
                MessageBox.Show("Wrong e-mail or password!");
                emailInput.Clear();
                passwordInput.Clear();
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
