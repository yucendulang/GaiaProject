using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia.MadAndroidSpecialBuildings
{
    public class Academy : Building
    {
        public override Type BaseBuilding => typeof(TradeCenter);
        public override int MagicLevel => 3 + MagicLevelIncrease;
    }

    public class StrongHold : Building
    {
        public override Type BaseBuilding => typeof(ResearchLab);
        public override int MagicLevel => 3 + MagicLevelIncrease;
    }
}
