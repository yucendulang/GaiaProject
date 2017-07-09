using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia.Tiles
{
    public static class ALTMgr
    {
        public static List<AllianceTile> GetList()
        {
            var result = new List<AllianceTile>()
            {
                new ALT1(),
                new ALT1(),
                new ALT1(),
                new ALT2(),
                new ALT2(),
                new ALT2(),
                new ALT3(),
                new ALT3(),
                new ALT3(),
                new ALT4(),
                new ALT4(),
                new ALT4(),
                new ALT5(),
                new ALT5(),
                new ALT5(),
                new ALT6(),
                new ALT6(),
                new ALT6()
            };
            return result;
        }
    }
    public abstract class AllianceTile:GameTiles
    {
    }
    public class ALT1 : AllianceTile
    {
        public override string desc
        {
            get
            {
                return "2K,6VP";
            }
        }
    }
    public class ALT2 : AllianceTile
    {
        public override string desc
        {
            get
            {
                return "6C,7VP";
            }
        }
    }
    public class ALT3 : AllianceTile
    {
        public override string desc
        {
            get
            {
                return "2O,7VP";
            }
        }
    }
    public class ALT4 : AllianceTile
    {
        public override string desc
        {
            get
            {
                return "2PW,8VP";
            }
        }
    }
    public class ALT5 : AllianceTile
    {
        public override string desc
        {
            get
            {
                return "1Q,8VP";
            }
        }
    }
    public class ALT6 : AllianceTile
    {
        public override string desc
        {
            get
            {
                return "12VP";
            }
        }
    }
    public class ALT7 : AllianceTile
    {
        public override string desc
        {
            get
            {
                return "1O,1K,2C";
            }
        }
    }
}
