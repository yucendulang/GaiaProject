using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public abstract class Building
    {
        public abstract Type BaseBuilding { get; }
        public abstract int MagicLevel { get; }
        public virtual string Name => GetType().Name;
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

    public class ResearchLab : Building
    {
        public override Type BaseBuilding => typeof(TradeCenter);
        public override int MagicLevel => 2;
    }

    public class Academy : Building
    {
        public int MagicLevelIncrease = 0;
        public override Type BaseBuilding => typeof(ResearchLab);
        public override int MagicLevel => 3 + MagicLevelIncrease;
    }

    public class StrongHold : Building
    {
        public int MagicLevelIncrease = 0;
        public override Type BaseBuilding => typeof(TradeCenter);
        public override int MagicLevel => 3 + MagicLevelIncrease;
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
        /// Academy 提供知识的
        /// </summary>
        AC1,
        /// <summary>
        /// 
        /// </summary>
        AC2,
        /// <summary>
        /// StrongHold
        /// </summary>
        SH
    }
}
