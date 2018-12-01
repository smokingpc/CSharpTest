using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;
using Newtonsoft.Json;

namespace JsonTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var data = GenerateTestData();

            textBox1.Text = JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public CMyData GenerateTestData()
        {
            CMyData data = new CMyData();
            data.Name = "MyData";
            data.Type = DATATYPE.PUBLIC;
            data.Id = 777;
            data.Payload1 = new Dictionary<string, int>() 
                            { 
                                { "達美樂", 28825252 }, 
                                { "PizzaHut", 23939889 } 
                            };
            data.Payload2 = new List<CFriend>();
            data.Payload2.Add(
                    new CFriend()
                    {
                        Name = "Kevin",
                        PostCode = 111,
                    });
            data.Payload2.Add(
                    new CFriend()
                    {
                        Name = "Leon",
                        PostCode = 999,
                    });

            var payload3 = new List<CFriend>();
            payload3.Add(new CFriend() { Name="XYZ", PostCode=456});
            data.Payload3 = payload3;

            var payload4 = new Dictionary<string, string>() 
                {
                    {"Marine", "gogogo!"},
                    {"BattleCruiser", "Who is calling the fleet?"}
                };
            data.Payload4= payload4;

            return data;
        }
    }

    public class CMyData
    {
        [JsonProperty("NAME")]
        public string Name { get; set; }
        [JsonProperty("TYPE")]
        public DATATYPE Type { get; set; }
        [JsonProperty("ID")]
        public Int64 Id { get; set; }

        [JsonProperty("PAYLOAD1")]
        public Dictionary<string, int> Payload1 { get; set; }

        [JsonProperty("LIST")]
        public List<CFriend> Payload2 { get; set; }

        [JsonProperty("XYZ")]
        public object Payload3 { get; set; }
        
        [JsonProperty("SC2")]
        public object Payload4 { get; set; }
    }
    public class CFriend
    { 
        [JsonProperty("LIST")]
        public string Name {get;set;}
        [JsonProperty("POSTCODE")]
        public Int32 PostCode { get; set; }
    }

    public enum DATATYPE
    { 
        UNKNOWN=0,
        PRIVATE= 1,
        PUBLIC=2,
    }
}
