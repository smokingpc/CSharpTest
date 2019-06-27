using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Mail;

namespace SmtpConnectTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = textBox3.Enabled = checkBox1.Checked;
        }


        private void SendMail(string server, int port, bool smtps, string receiver, NetworkCredential credential, string subject, string content)
        {
            try
            {
                using (var smtp = new SmtpClient(server, port))
                using (var mail = new MailMessage())
                {
                    smtp.EnableSsl = smtps;
                    smtp.DeliveryFormat = SmtpDeliveryFormat.International;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    if (null != credential)
                    {
                        //用gmail或smtp.outlook.com時必須開TLS並且要authentication才能寄信
                        //但注意那個authentication帳號不能開2-factor驗證，不然會鬼打牆的回你 "Authentication required"
                        //(這是資訊隱蔽，故意鬼打牆)
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = credential;
                    }
                    

                    mail.Body = content;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = false;

                    //寄件人這欄位在outlook365以及gmail的smtp這邊，必須跟登入帳號一樣，
                    //否則會被踢下來拒絕發信
                    mail.From = new MailAddress(credential.UserName, "SMTP測試小工具");
                    mail.To.Add(new MailAddress(receiver));
                    mail.Subject = subject;
                    

                    smtp.Send(mail);
                    MessageBox.Show("SMTP sendmail ok!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SMTP sendmail failed. Msg="+ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void InitUI()
        {
            ClearAll();
            checkBox1.Checked = true;
            //checkBox1.Checked = true;
            textBox1.Text = "smtp.google.com";
            textBox2.Text = "mytest@google.com";
            textBox3.Text = "";
            textBox4.Text = "my_receiver@google.com";
            textBox5.Text = "SMTP connection testing mail";
            textBox6.Text = "come on, I am just a testing mail.";
            textBox7.Text = "587";
        }

        private void ClearAll()
        {
            textBox1.Text = textBox2.Text = textBox3.Text =
                    textBox4.Text = textBox5.Text = textBox6.Text = textBox7.Text = "";
            checkBox1.Checked = checkBox2.Checked = false;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitUI();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string server = textBox1.Text;
            string login_id = textBox2.Text;
            string login_pwd = textBox3.Text;
            string target = textBox4.Text;
            string subject = textBox5.Text;
            string content = textBox6.Text;
            int port = int.Parse(textBox7.Text);

            NetworkCredential cred = null;
            if (checkBox1.Checked)
            {
                cred = new NetworkCredential(login_id, login_pwd);
            }
            SendMail(server, port, checkBox2.Checked, target, cred, subject, content);
        }
    }
}
