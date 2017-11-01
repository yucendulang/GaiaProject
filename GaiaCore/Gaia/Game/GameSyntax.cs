using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GaiaCore.Gaia
{
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
        public static Regex buildRegex = new Regex(build + " [a-z][0-9]{1,2}");
        /// <summary>
        /// 玩家命令正则
        /// </summary>
        public static Regex commandRegex = new Regex("[a-z]+:.+");

    }
}
