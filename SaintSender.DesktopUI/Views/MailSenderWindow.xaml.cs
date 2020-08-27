using SaintSender.DesktopUI.ViewModels;
using System.Windows;

namespace SaintSender.DesktopUI.Views
{
    public partial class MailSenderWindow : Window
    {
        MainViewModel _vm;

        public MailSenderWindow()
        {
            InitializeComponent();
            _vm = MainViewModel.GetInstance();
            this.DataContext = _vm;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(To.Text) || string.IsNullOrEmpty(Subject.Text) || string.IsNullOrEmpty(Message.Text))
            {
                MessageBox.Show("You have to fill all boxes!"); 
            }
            else
            {
                _vm.SendEmail(To.Text, Subject.Text, Message.Text);
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to quit?", "Sure about that?", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    return;
            }
        }
    }
}
