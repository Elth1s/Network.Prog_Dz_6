using EAGetMail;
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

namespace Network.Prog_Дз_6_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailTextBox.Text) || string.IsNullOrEmpty(PasswordBox.Password)) return;

            MailServer server = new MailServer(
                "imap.gmail.com",
                EmailTextBox.Text,
                PasswordBox.Password,
                ServerProtocol.Imap4)
            {
                SSLConnection = true,
                Port = 993
            };

            MailClient client = new MailClient("TryIt"); // trial version

            try
            {
                client.Connect(server);

                // show all folders
                foreach (var f in client.GetFolders())
                {
                    MyListBox.Items.Add(f.Name);
                    foreach (var subF in f.SubFolders)
                    {
                        MyListBox.Items.Add("\t" + subF.Name);
                    }
                }

                //// select Trash folder
                //client.SelectFolder(client.Imap4Folders[1].SubFolders[2]);
                //// get mails in selected folder
                //var messages = client.GetMailInfos();

                //foreach (var m in messages)
                //{

                //    Console.WriteLine($"Index: {m.Index}{Environment.NewLine}Size: {m.Size}\n");

                //    Mail message = client.GetMail(m);

                //    if (m.Index == 21)
                //    {
                //        // save attachment
                //        message.Attachments[0].SaveAs(message.Attachments[0].Name, true);
                //    }

                //    Console.WriteLine($"From: {message.From}\n\n\t{message.Subject}");
                //    Console.WriteLine($"Date: {message.SentDate}\tAttachments: {message.Attachments.Count()}");
                //    Console.WriteLine($"Body: {new string(message.TextBody.Take(50).ToArray())}...");
                //    Console.WriteLine("-----------------------------------------------------");
                //}

            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }
    }
}
