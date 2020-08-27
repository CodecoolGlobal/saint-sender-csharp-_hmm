using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using SaintSender.Core.Entities;
using SaintSender.DesktopUI.ViewModels;
using SaintSender.DesktopUI.Views;

namespace SaintSender.DesktopUI
{
    public partial class MainWindow : Window
    {
        MainViewModel _vm;
        private DispatcherTimer dispatcher = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            _vm = MainViewModel.GetInstance();
            this.DataContext = _vm;          
            if (_vm.IsConnectedToInternet()) { connectionContent.Text = "Connected to internet"; }
            if (User.HaveAlreadyLoggedInUser()) { _vm.HandleLogIn(User.GetSavedUsername(), User.GetSavedpassword()); }
            else { _vm.LoginButtonContent = "Login"; }

            Thread periodicThread = new Thread(PeriodicStartingThread);
            periodicThread.Start();
        }

        private void PeriodicStartingThread()
        {
            dispatcher.Interval = TimeSpan.FromSeconds(5);
            dispatcher.Tick += PeriodicEmailChecking;
            dispatcher.Start();
        }

        private void PeriodicEmailChecking(object sender, EventArgs e)
        {
            if (_vm.IsSomeoneLoggedIn() && _vm.IsConnectedToInternet())
            {
                _vm.CheckForNewEmails();
                connectionContent.Text = "Connected to internet";
            }
            else if (!_vm.IsConnectedToInternet()) { connectionContent.Text = "No internet connection"; }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.LoginButtonContent.Equals("Login")) { new LoginWindow().Show(); }
            else { _vm.HandleLogout(); }
        }
        
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem clicked = sender as ListViewItem;
            _vm.PutEmailIntoSelectedField(int.Parse(clicked.Tag.ToString()));
            new MailWatcher().Show();
        }

        private void Compose_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.IsSomeoneLoggedIn()) { new MailSenderWindow().Show(); }
            else { MessageBox.Show("You must be logged in to send emails!", "Please Log in"); }
        }
    }
}
