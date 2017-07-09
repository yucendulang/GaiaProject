using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Util
{
    public static class RandomInstance
    {
        private static Random instance;
        static RandomInstance()
        {
            instance = new Random();
        }
        public static Random GetInstance()
        {
            return instance;
        }

        public static int Next(int i)
        {
            return instance.Next(i);
        }

    }
}
