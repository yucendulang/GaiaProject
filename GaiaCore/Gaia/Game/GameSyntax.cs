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
        public static Regex RBTRegex = new Regex("\\+rbt[0-9]{1,2}");
        /// <summary>
        /// Update语句正则
        /// </summary>
        public const string upgrade = "upgrade";
        public static Regex upgradeRegex = new Regex(upgrade + " ([a-z][0-9]{1,2}) to ([a-z0-9]{1,3})");
        /// <summary>
        /// 吸魔力的正则表达式
        /// </summary>
        public const string leech = "leech";
        public const string decline = "decline";
        public static Regex leechPowerRegex = new Regex("[a-z]+:(leech|decline) ([0-9]) from (.+)");
        /// <summary>
        /// pass板子的正则
        /// </summary>
        public const string pass = "pass";
        public static Regex passRegex = new Regex("pass (rbt[0-9]{1,2})");
        /// <summary>
        /// 盖亚语句
        /// </summary>
        public const string gaia = "gaia";
        public static Regex gaiaRegex = new Regex(gaia + " ([a-z][0-9]{1,2})");
        public const string action = "action";
        private static readonly List<string> turnActionList = new List<string>()
        {
            "act10",
            "stt1",
            "att1",
            "att2",
            "att3",
            "act1",
            "act3",
            "act4",
            "act5",
            "act7",
            "act8",
            "act9",
            "ac2",
        };
        public static Regex actionRegex = new Regex(string.Format("{0} ({1})", action, string.Join("|", turnActionList)));
        /// <summary>
        /// 建立城邦
        /// </summary>
        public static string satellite = "satellite";
        public static Regex forgingAlliance = new Regex("satellite [a-z][0-9]{1,2}(,[a-z][0-9]{1,2})*");
        public static string alliance = "alliance";
        public static Regex forgingAlliance2 = new Regex(alliance+" [a-z][0-9]{1,2}(,[a-z][0-9]{1,2})*");

    }

    public static class GameFreeSyntax
    {
        public static List<Regex> GetRegexList()
        {
            var ret = new List<Regex>();
            ret.Add(burningRegex);
            ret.Add(getTechTilesRegex);
            ret.Add(actionRegex);
            ret.Add(ALTRegex);
            ret.Add(ConvertRegex);
            ret.Add(ReturnTechTilesRegex);
            ret.Add(PlanetRegex);
            ret.Add(advTechRegex2);
            ret.Add(NoAdvanceTechTrack);
            return ret;
        } 
        public static Regex burningRegex = new Regex("burn ([0-9])");
        public static Regex QICShip = new Regex("qicship ([0-9])");
        public static Regex getTechTilesRegex = new Regex("\\+(a|s)tt([0-9]{1,2})");
        public static Regex advTechRegex2 = new Regex("advance (tf|ai|eco|gaia|sci|ship)");
        
        private static readonly List<string> turnActionList = new List<string>()
        {
            "rbt1",
            "rbt2",
            "act2",
            "act6",
        };
        public static Regex actionRegex = new Regex(string.Format("{0} ({1})", GameSyntax.action, string.Join("|", turnActionList)));
        /// <summary>
        /// 获取AllianceTile正则
        /// </summary>
        public static Regex ALTRegex = new Regex("\\+(alt[0-9])");
        /// <summary>
        /// convert的正则
        /// </summary>
        public static Regex ConvertRegex = new Regex("convert ([0-9]{1,2})([a-z]{1,3}) to ([0-9]{1,2})([a-z]{1,3})");
        /// <summary>
        /// 退回板子的正则
        /// </summary>
        public static Regex ReturnTechTilesRegex = new Regex("-stt[1-9]{1}");
        public static string planet = "planet";
        public static Regex PlanetRegex = new Regex(planet + " ([a-z][0-9]{1,2})");
        public static Regex NoAdvanceTechTrack = new Regex("-advance");
    }
}
