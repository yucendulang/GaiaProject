using System;
using System.Collections.Generic;
using System.Text;
using GaiaCore.Util;

namespace GaiaCore.Gaia.Tiles
{
    public static class ATTMgr
    {
        /// <summary>
        /// 获取随机N个ATT板块
        /// </summary>
        /// <param name="i"></param>
        public static List<AdavanceTechnology> GetRandomList(int n,Random random)
        {
            var list = new List<AdavanceTechnology>()
            {
                new ATT1(),
                new ATT2(),
                new ATT3(),
                new ATT4(),
                new ATT5(),
                new ATT6(),
                new ATT7(),
                new ATT8(),
                new ATT9(),
                new ATT10(),
                new ATT11(),
                new ATT12(),
                new ATT13(),
                new ATT14(),
                new ATT15(),
            };

            var result = new List<AdavanceTechnology>();
            for(int i = 0; i < n; i++)
            {
                result.Add(list.RandomRemove(random));
            }
            return result;
        }
    }
    public abstract class AdavanceTechnology:GameTiles
    {
    }

    public class ATT1 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "Act:3O";
            }
        }
    }
    public class ATT2 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "Act:3K";
            }
        }
    }
    public class ATT3 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "Act:1QIC,5C";
            }
        }
    }
    public class ATT4 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "M>>3VP";
            }
        }
    }
    public class ATT5 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "TC>>3VP";
            }
        }
    }
    public class ATT6 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "RA>>2VP";
            }
        }
    }
    public class ATT7 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "pass-vp:AL*3";
            }
        }
    }
    public class ATT8 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "pass-vp:P_type*1";
            }
        }
    }
    public class ATT9 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "pass-vp:RL*2";
            }
        }
    }
    public class ATT10 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1SC->1O";
            }
        }
    }
    public class ATT11 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1M->2VP";
            }
        }
    }
    public class ATT12 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1TC->4VP";
            }
        }
    }
    public class ATT13 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1G->2VP";
            }
        }
    }
    public class ATT14 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1SC->2VP";
            }
        }
    }
    public class ATT15 : AdavanceTechnology
    {
        public override string desc
        {
            get
            {
                return "1AL->5VP";
            }
        }
    }   
}
