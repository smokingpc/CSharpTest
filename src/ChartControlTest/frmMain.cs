using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartControlTest
{
    public partial class frmMain : Form
    {
        //private void 

        public frmMain()
        {
            InitializeComponent();
        }

        private void ShowChart2()
        {
            List<CMyData> datalist = new List<CMyData>();
            
            chart1.DataSource = datalist;
            chart1.Series[0].XValueMember = "data3";
            chart1.Series[0].YValueMembers = "data4";

            datalist.Add(new CMyData(1, 14));
            datalist.Add(new CMyData(2, 17));
            datalist.Add(new CMyData(3, 13));
            
            chart1.DataBind();
        }

        private void ShowChart1()
        {
            List<CMyData> datalist = new List<CMyData>();
            datalist.Add(new CMyData(1, 14));
            datalist.Add(new CMyData(2, 17));
            datalist.Add(new CMyData(3, 13));

            //每個chart可以擁有多個圖形同時顯示
            //每個圖形視為一組數列
            //每組數列可以有不同的value/argument member
            var series = chartControl1.Series[0];
            //series.DataSource = ds;
            //chartControl1.SeriesDataMember
            series.ValueDataMembers.AddRange(new string[] { "data3", "data2" }); //Y軸
            series.ArgumentDataMember = "data4";    //X軸
            series.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            series.ValueScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            series.DataSource = datalist;   //會自動照X軸排序而不是照list順序.......
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ShowChart1();
            ShowChart2();
        }


    }

    public class CMyData
    {
        //只有public auto property可以當作value / argument member
        public int data1 { get; set; }
        public int data2 { get; set; }
        
        public int data3{ get; set; }
        public int data4{ get; set; }

        public CMyData(int d1, int d2) 
        { 
            data1 = d1; data2 = d2-4;
            data3 = d1; data4 = d2; 
        }
    }
}
