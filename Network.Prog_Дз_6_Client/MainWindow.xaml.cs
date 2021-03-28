using EAGetMail;
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

namespace Network.Prog_Дз_6_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<MyMailMessage> mailMessages = new ObservableCollection<MyMailMessage>();
        bool isConnected = false;
        public MainWindow()
        {
            InitializeComponent();
            MyListBox.ItemsSource = mailMessages;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailTextBox.Text) || string.IsNullOrEmpty(PasswordBox.Password)) return;

            MailServer server = new MailServer(
                "",
                EmailTextBox.Text,
                PasswordBox.Password,
                ServerProtocol.Imap4)
            {
                SSLConnection = true,
            };
            if (GoogleRadioButton.IsChecked == true) 
            {
                server.Server = "imap.gmail.com";
                server.Port = 993;
            }
            else if (UkrRadioButton.IsChecked == true)
            {
                server.Server = "imap.ukr.net";
                server.Port = 993;
            }
            MailClient client = new MailClient("TryIt"); // trial version

            try
            {
                client.Connect(server);
                var messages = client.GetMailInfos();

                    foreach (var m in messages)
                    {
                        Mail message = client.GetMail(m);

                        mailMessages.Add(new MyMailMessage() { From = message.From.ToString(), Subject = message.Subject, AllText = message.TextBody, Text = new string(message.TextBody.Take(80).ToArray()).Replace("\n", "").Replace("\r", "") + "...", Date = message.ReceivedDate });
                    }
                isConnected = true;
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if(isConnected==true)
            {
                string server = "";
                if (GoogleRadioButton.IsChecked == true)
                {
                    server = "smtp.gmail.com";
                }
                else if (UkrRadioButton.IsChecked == true)
                {
                    server = "smtp.ukr.net";
                }
                SendMessage sendMessage = new SendMessage(server, EmailTextBox.Text, PasswordBox.Password, "");
                sendMessage.ShowDialog();
            }
        }

        private void Reply_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected == true)
            {
                string server = "";
                if (GoogleRadioButton.IsChecked == true)
                {
                    server = "smtp.gmail.com";
                }
                else if (UkrRadioButton.IsChecked == true)
                {
                    server = "smtp.ukr.net";
                }
                string to = "";
                for (int i = 0; i < mailMessages[MyListBox.SelectedIndex].From.Length; i++)
                {
                    if(mailMessages[MyListBox.SelectedIndex].From[i] == '<')
                    {
                        for (int j = i+1; j < mailMessages[MyListBox.SelectedIndex].From.Length; j++)
                        {
                            if (mailMessages[MyListBox.SelectedIndex].From[j] != '>')
                                to += mailMessages[MyListBox.SelectedIndex].From[j];
                            else
                                break;
                        }
                    }
                }
                SendMessage sendMessage = new SendMessage(server, EmailTextBox.Text, PasswordBox.Password, to);
                sendMessage.ShowDialog();
            }
        }

        private void CheckInfoButton_Click(object sender, RoutedEventArgs e)
        {
            MyMailMessage message = mailMessages[MyListBox.SelectedIndex];
            MessageBox.Show($"From: {message.From}\nDate: {message.Date}\n{message.AllText}");
        }
    }
}
