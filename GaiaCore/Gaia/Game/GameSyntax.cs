using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GaiaCore.Gaia
{
    /// <summary>
    /// 语义分析进来全部小写
    /// </summary>
    public static class GameSyntax
    {
        /// <summary>
        /// 游戏开局的语义
        /// </summary>
        public const string setupGame = "setupgame seed";
        public static Regex setupGameRegex = new Regex(setupGame + "[0-9]+");
        /// <summary>
        /// Faction selection
        /// </summary>
        public const string factionSelection = "setup";
        public static Regex factionSelectionRegex = new Regex(factionSelection + " [a-z]+");
        ///<summary>
        public const string build = "build";
        public static Regex buildRegex = new Regex(build + " ([a-z][0-9]{1,2})");
        /// <summary>
        /// 玩家命令正则
        /// </summary>
        public static Regex commandRegex = new Regex("[a-z]+:.+");
        /// <summary>
        /// 获取RoundBooster正则
        /// </summary>
        public const string rbt = "+rbt";
        public static Regex RBTRegex = new Regex("\\+rbt[0-9]");
        /// <summary>
        /// Update语句正则
        /// </summary>
        public const string upgrade = "upgrade";
        public static Regex upgradeRegex = new Regex(upgrade + " ([a-z][0-9]{1,2}) to ([a-z]{1,2})");
        /// <summary>
        /// 吸魔力的正则表达式
        /// </summary>
        public const string leech = "leech";
        public const string decline = "decline";
        public static Regex leechPowerRegex = new Regex("[a-z]+:(leech|decline) ([0-9]) from (.+)");
    }

    public static class GameFreeSyntax
    {
        public static List<Regex> GetRegexList()
        {
            var ret = new List<Regex>();
            ret.Add(burningRegex);
            return ret;
        } 
        public static Regex burningRegex = new Regex("burn ([0-9])");
        public static Regex transformRegex = new Regex("transform ([0-9])");
    }
}
