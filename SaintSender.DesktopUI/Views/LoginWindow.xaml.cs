using SaintSender.DesktopUI.ViewModels;
using System;
using System.IO;
using System.Windows;

namespace SaintSender.DesktopUI.Views
{
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
            if (_vm.HandleLogIn(emailInput.Text, passwordInput.Password)) { this.Close(); }
            else 
            {
                _vm.LoginButtonContent = "Login";
                File.WriteAllText("./data/user.txt", String.Empty);
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
