using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace HexValueConverter
{
    public partial class frmMain
    {
        private string ConvertValues(Int64 input)
        {
            return input.ToString("X16");
        }
        private string ConvertValues(double input)
        {
            Int64 data = BitConverter.DoubleToInt64Bits(input);
            return data.ToString("X16");
        }
        private string ConvertValues(string hex, VALUE_TYPE type)
        {
            string result = "";

            string input = hex.Replace(" ","");

            Int64 intermediate = Convert.ToInt64(input, 16);

            if (type == VALUE_TYPE.INT32_INT64)
                result = intermediate.ToString();
            else
                result = BitConverter.Int64BitsToDouble(intermediate).ToString("F8", CultureInfo.InvariantCulture);

            return result;
        }
    }
}
