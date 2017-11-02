using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public abstract class Building
    {
        public abstract Type BaseBuilding { get; }
        public abstract int MagicLevel { get; }
    }

    public class Mine : Building
    {
        public override Type BaseBuilding => null;
        public override int MagicLevel => 1;
    }
    public class TradeCenter : Building
    {
        public override Type BaseBuilding => typeof(Mine);
        public override int MagicLevel => 2;
    }

    public class ReaserchLab : Building
    {
        public override Type BaseBuilding => typeof(TradeCenter);
        public override int MagicLevel => 2;
    }

    public class Academy : Building
    {
        public override Type BaseBuilding => typeof(ReaserchLab);
        public override int MagicLevel => 3;
    }

    public class StrongHold : Building
    {
        public override Type BaseBuilding => typeof(TradeCenter);
        public override int MagicLevel => 3;
    }

    public enum BuildingSyntax
    {
        /// <summary>
        /// mine
        /// </summary>
        M,
        /// <summary>
        /// TradeCenter
        /// </summary>
        TC,
        /// <summary>
        /// ResearchLab
        /// </summary>
        RL,
        /// <summary>
        /// Academy
        /// </summary>
        AC,
        /// <summary>
        /// StrongHold
        /// </summary>
        SH
    }
}
