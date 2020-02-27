using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace MessagePackTest
{
    internal static class Extension
    {
        internal static void SetText(this TextBox tb, string msg, bool append = true)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action)(() => { SetText(tb, msg, append); }));
            }
            else
            {
                if (append)
                {
                    tb.AppendText(msg);
                }
                else
                    tb.Text = msg;
            }
        }

        internal static void SetLine(this TextBox tb, string msg, bool append = true)
        {
            msg = msg + "\r\n";
            tb.SetText(msg, append);
        }
    }
}
