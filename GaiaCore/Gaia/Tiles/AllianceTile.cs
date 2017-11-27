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
    public abstract class AllianceTile : GameTiles
    {
        public AllianceTile()
        {
            this.typename = "alt";
            this.name = this.GetType().Name;
        }
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
        public override bool OneTimeAction(Faction faction)
        {
            faction.Knowledge += 2;
            faction.Score += 6;
            return true;
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

        public override bool OneTimeAction(Faction faction)
        {
            faction.Credit += 6;
            faction.Score += 7;
            return true;
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

        public override bool OneTimeAction(Faction faction)
        {
            faction.Ore += 2;
            faction.Score += 7;
            return true;
        }
    }
    public class ALT4 : AllianceTile
    {
        public override string desc
        {
            get
            {
                return "2PWT,8VP";
            }
        }

        public override bool OneTimeAction(Faction faction)
        {
            faction.PowerToken1 += 2;
            faction.Score += 8;
            return true;
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
        public override bool OneTimeAction(Faction faction)
        {
            faction.QICs += 1;
            faction.Score += 8;
            return true;
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

        public override bool OneTimeAction(Faction faction)
        {
            faction.Score += 12;
            return true;
        }

        public override bool IsUsed { get => true; set => base.IsUsed = value; }
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

        public override bool OneTimeAction(Faction faction)
        {
            faction.Ore += 1;
            faction.Knowledge += 1;
            faction.Credit += 2;
            return true;
        }
    }
}
