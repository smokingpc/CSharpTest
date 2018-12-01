using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLTest
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }

        private string GetChileLocationName(XmlNode node)
        {
            var child = node.FirstChild;
            while (child != null)
            {
                if (child.Name == "locationName")
                    return child.InnerText;
                child = child.NextSibling;
            }

            return "";
        }

        private void ParseXML2(string filename)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("my_ns", "urn:cwb:gov:tw:cwbcommon:0.1");
            //var result = doc.SelectNodes("//my_ns:location", mgr);
            //var result2 = doc.SelectNodes("//my_ns:location[my_ns:locationName=\"基隆\"]", mgr);
            var result = doc.SelectSingleNode("//my_ns:location[my_ns:locationName=\"基隆\"]", mgr);

        }

        private void ParseXML(string filename)
        {
            List<string> locations = new List<string>();

            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNode node = doc.FirstChild;
            node = node.NextSibling;
            //XmlNode sibling = node.NextSibling;
            if (node.Name == "cwbopendata")
            {
                node = node.FirstChild;
                while (node != null)
                { 
                    if(node.Name == "location")
                    {
                        var result = GetChileLocationName(node);
                        if(result != "")
                            locations.Add(result);
                    }
                    node = node.NextSibling;
                }
            }
            foreach (var item in locations)
                textBox2.AppendText(item + "\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML Files(*.xml)|*.xml";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                textBox1.Text = dlg.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
                ParseXML2(textBox1.Text);
        }
    }
}
