using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppConfigTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            var name = config.AppSettings.Settings["Name"];
            var tel = config.AppSettings.Settings["Tel"];
            var foods = config.AppSettings.Settings["Foods"];

            if (null != name)
                textBox1.Text = name.Value;

            if (null != tel)
                textBox2.Text = tel.Value;

            var foodlist = foods.Value.Split(',');
            textBox3.Text = foodlist[0];
            textBox4.Text = foodlist[1];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string tel = textBox2.Text;

            //ConfigurationUserLevel.None will open "AppConfigTest.exe.Config" file in exe path.
            //  ConfigurationUserLevel.PerUserRoamingAndLocal and  ConfigurationUserLevel.PerUserRoaming
            //  will open roaming config in user profile path.
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;
            if(null != settings["Name"])
                settings["Name"].Value = name;  //update value
            else
                settings.Add("Name", name);   //add value

            if (null != settings["Tel"])
                settings["Tel"].Value = tel;
            else
                settings.Add("Tel", tel);

            string data = textBox3.Text + "," + textBox4.Text;
            if (null != settings["Foods"])
                settings["Foods"].Value = data;
            else
                settings.Add("Foods", data);
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
