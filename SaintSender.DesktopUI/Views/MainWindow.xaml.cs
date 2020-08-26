using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenPop.Mime;
using SaintSender.Core.Entities;
using SaintSender.Core.Services;
using SaintSender.DesktopUI.ViewModels;
using SaintSender.DesktopUI.Views;

namespace SaintSender.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            _vm = MainViewModel.GetInstance();
            this.DataContext = _vm;
            User user = new User();
            if (user.HaveAlreadyLoggedInUser())
            {
                string emailInput = user.GetSavedUsername();
                string passwordInput = user.GetSavedpassword();
                _vm.LoginButtonContent = "Logout";
                _vm.handleLogIn(emailInput, passwordInput);
            }
            else
            {
                _vm.LoginButtonContent = "Login";
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.LoginButtonContent.Equals("Login"))
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }
            else
            {
                _vm.handleLogout();
            }
           
        }
        
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem clicked = sender as ListViewItem;
            _vm.PutEmailIntoSelectedField(int.Parse(clicked.Tag.ToString()));

            MailWatcher mailWatcher = new MailWatcher();
            mailWatcher.Show();
        }

        private void Compose_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.isSomeoneLoggedIn())
            {
                MailSenderWindow senderWindow = new MailSenderWindow();
                senderWindow.Show();
            }
            else
            {
                MessageBox.Show("You must be logged in to send emails!");
            }
        }
    }
}
