using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Util
{
    public static class StringExtensions
    {
        public static int ParseToInt(this string source,int defaultValue = 0)
        {
            if(int.TryParse(source,out int result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        } 
    }
}
