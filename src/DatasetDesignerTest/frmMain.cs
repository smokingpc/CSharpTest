using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatasetDesignerTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();


            using (jammingDataContext db = new jammingDataContext())
            { 
                var result = from alldata in db.FREQ_TYPEs select alldata; 

            }
        }
    }
}
