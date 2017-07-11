using System;
using System.Collections.Generic;
using System.Text;
using GaiaCore.Util;

namespace GaiaCore.Gaia.Tiles
{
    public static class STTMgr
    {
        public static List<StandardTechnology> GetRandomList(int n,Random random)
        {
            var list = new List<StandardTechnology>()
            {
                new STT1(),
                new STT2(),
                new STT3(),
                new STT4(),
                new STT5(),
                new STT6(),
                new STT7(),
                new STT8(),
                new STT9()
            };
            var result = new List<StandardTechnology>();
            for (int i = 0; i < n; i++)
            {
                result.Add(list.RandomRemove(random));
            }
            return result;
        }

        public static List<StandardTechnology> GetOtherList(List<StandardTechnology> removelist)
        {
            var list = new List<StandardTechnology>()
            {
                new STT1(),
                new STT2(),
                new STT3(),
                new STT4(),
                new STT5(),
                new STT6(),
                new STT7(),
                new STT8(),
                new STT9()
            };
            list.RemoveAll(x => removelist.Exists(y=>x.GetType()==y.GetType()));
            return list;
        }
    }


    public abstract class StandardTechnology : GameTiles
    {

    }
    /// <summary>
    /// 行动：获得4魔力
    /// </summary>
    public class STT1 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "Act:4PW";
            }
        }
    }
    /// <summary>
    /// 每当在盖亚星球建造矿场，获得3分
    /// </summary>
    public class STT2 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "M(G) >> 3VP";
            }
        }
    }
    /// <summary>
    /// 获得7分
    /// </summary>
    public class STT3 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "7VP";
            }
        }
    }
    /// <summary>
    /// 每登录1个星球种类，为你获得1点知识
    /// </summary>
    public class STT4 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "1 P_type->1K";
            }
        }
    }
    public class STT5 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "1O,1Q";
            }
        }
    }
    public class STT6 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "+4C";
            }
        }
    }
    public class STT7 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "+1O,+1PW";
            }
        }
    }
    public class STT8 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "+1K,1C";
            }
        }
    }
    /// <summary>
    /// 学院要塞等级+1
    /// </summary>
    public class STT9 : StandardTechnology
    {
        public override string desc
        {
            get
            {
                return "SH/AD_PWLV+1";
            }
        }
    }
}
