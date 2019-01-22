using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputValidateTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            dxErrorProvider1.SetErrorType();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("textBox1_Validating()");
            // Validation 是在 LostFocus前先檢查整個text是否合乎格式
            if ("" == textBox1.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox1, "Please input your name!!");
            }
        }

        private void textEdit1_Validating(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("textEdit1_Validating()");

            //TextEdit的 Mask 是在 keydown 就作用，過濾掉不合規範的字元
            //但 Validation 是在 LostFocus前先檢查整個text是否合乎格式
            // 例如 電話欄位  區域號碼是數字，但沒有 1234 這種區域號碼
            // 這就要在 Validation 去檢查

            //Validation 分為 Validating / Validated 兩個event
            //一個是 pre-event , 一個是 post-event
            if ("" == textEdit1.Text)
            {
                e.Cancel = true;
                dxErrorProvider1.SetError(textBox1, "Please input your phone number!!");
            }
            else
            { 
                
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("textBox2_Validating()");
            // Validation 是在 LostFocus前先檢查整個text是否合乎格式
            try
            {
                int age = int.Parse(textBox2.Text);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBox2, "Please input correct Age!!");
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("button1_Click()");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("frmMain_FormClosing()");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x10)
            {
                textBox1.CausesValidation = textBox2.CausesValidation = textEdit1.CausesValidation = false;
                System.Diagnostics.Debug.WriteLine("Disable CauseValidation in all input.");
            }
            base.WndProc(ref m);
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBox1, "");
        }
    }
}
