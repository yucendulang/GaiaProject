using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaProject2.Gaia
{
    /// <summary>
    /// 每一局游戏实例化一个Game
    /// </summary>
    public class GaiaGame
    {
        public GaiaGame()
        {
            Map = MapMgr.GetRandomMap();
        }
        /// <summary>
        /// 在游戏开始的时候实例化一个Map
        /// </summary>
        Map Map { set; get; }
        /// <summary>
        /// 实例化四个玩家
        /// </summary>
        List<Faction> FactionList { set; get; }
        /// <summary>
        /// 游戏的一些公共状态包括现在第几轮轮到哪个玩家行动等等
        /// </summary>
        GameStatus GameStatus { set; get; }
    }
}
