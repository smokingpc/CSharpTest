using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConditionDispatcherTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void InitUI()
        {
            JObject json = new JObject();
            json.Add("operator", ">");
            json.Add("value", "24");
            json.Add("msg", "適溫");
            json.Add("is_alert", false);
 

            textBox1.Text = "24.5";
            textBox2.Text = json.ToString();
            textBox3.Text = "";
        }
        private void ClearAll()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = "";
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {
            ClearAll();
            InitUI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JObject json = JObject.Parse(textBox2.Text);
            var op = json["operator"].ToString();
            float threshold = float.Parse(json["value"].ToString());
            float value = float.Parse(textBox1.Text);
            var compare_func = CConditionMap.CondMap[op];
            if (null != compare_func)
            {
                var result = compare_func(value, threshold);

                textBox3.Text = string.Format("value({0:F1}) compare result : [{1}] || ",
                        value, result);

                bool alert = bool.Parse(json["is_alert"].ToString());
                textBox3.AppendText("ALERT? ==> " + (alert & result));
            }
            else
                textBox3.Text = "No matched Condition";
        }
    }

    public class CConditionMap
    {
        //運算符號與對應算式的MAP
        public static Dictionary<string, Func<float, float, bool>> CondMap =
                    new Dictionary<string, Func<float, float, bool>>() 
                    { 
                        {">", new Func<float, float, bool>(GT)},
                        {">=", new Func<float, float, bool>(GE)},
                        {"==", new Func<float, float, bool>(EQ)},
                    };

        //"greater than"
        public static bool GT(float value, float threshold) 
        {
            bool result = false;
            if (value > threshold)
                result = true;
            return result; 
        }
        //"greater than and equal"
        public static bool GE(float value, float threshold)
        {
            bool result = false;
            if (value >= threshold)
                result = true;
            return result;
        }
        //"equal to"
        public static bool EQ(float value, float threshold)
        {
            bool result = false;
            if (value == threshold)
                result = true;
            return result;
        }
    }
}
