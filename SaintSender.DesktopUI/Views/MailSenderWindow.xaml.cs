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
    /// Interaction logic for MailSenderWindow.xaml
    /// </summary>
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
            string emailTo = To.Text;
            string mailSubject = Subject.Text;
            string mailBody = Message.Text;
            _vm.SendEmail(emailTo, mailSubject, mailBody);

            this.Close();
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
