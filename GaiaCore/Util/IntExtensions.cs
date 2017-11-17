using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Util
{
    public static class IntExtensions
    {

        public static string ConvertPosToStr(int x1,int x2)
        {
            return Convert.ToChar((x1 + Convert.ToByte('A'))) + x2.ToString();
        }

        public static string ConvertPosToStr(Tuple<int,int> t)
        {
            return ConvertPosToStr(t.Item1, t.Item2);
        }
    }
}
