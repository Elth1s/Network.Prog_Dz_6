using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;

namespace Network.Prog_Дз_6_Client
{
    /// <summary>
    /// Interaction logic for SendMessage.xaml
    /// </summary>
    public partial class SendMessage : Window
    {
        int gmailPort = int.Parse(ConfigurationManager.AppSettings["gmail_port"]); //sets the server port
        int ukrPort = int.Parse(ConfigurationManager.AppSettings["ukr_port"]); //sets the server port
        string server; string mail; string password;
        public SendMessage(string server, string mail, string password, string to)
        {
            InitializeComponent();
            this.server = server;
            this.mail = mail;
            this.password = password;
            ToTextBox.Text = to;
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|Text documents (*.txt)|*.txt";
            dlg.FilterIndex = 1;
            dlg.InitialDirectory = @"D:\Images";

            if (dlg.ShowDialog() == true)
            {
                listBox.Items.Add(dlg.FileName);
            }
        }

        private void DeleteFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                listBox.Items.Remove(listBox.SelectedItem);
                listBox.SelectedIndex = -1;
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ThemeTextBox.Text) && !string.IsNullOrEmpty(ToTextBox.Text))
            {
                MailMessage message = new MailMessage(mail, ToTextBox.Text, ThemeTextBox.Text, BodyTextBox.Text);

                if (ImportantCheckBox.IsChecked == true)
                    message.Priority = MailPriority.High;
                foreach (var item in listBox.Items)
                {
                    message.Attachments.Add(new Attachment(item.ToString()));
                }

                // create a send object
                SmtpClient client;
                if(server.Contains("gmail"))
                    client = new SmtpClient(server, gmailPort);
                else
                    client = new SmtpClient(server, ukrPort);


                client.EnableSsl = true;

                // settings for sending mail
                client.Credentials = new NetworkCredential(mail, password);

                client.SendCompleted += Client_SendCompleted;

                // call asynchronous message sending
                client.SendAsync(message, "blabla");
            }
        }
        private void Client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show($"Message was sent! Token:{e.UserState}");
        }
    }
}
