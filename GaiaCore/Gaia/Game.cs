using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaCore.Util;

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
            GameStatus = new GameStatus();
            FactionList = new List<Faction>();
            ATTList= (from items in ATTMgr.GetRandomList(6) orderby items.GetType().Name.Remove(0,3).ParseToInt(-1) select items).ToList();
            STT6List = STTMgr.GetRandomList(6);
            STT3List = (from items in STTMgr.GetOtherList(STT6List) orderby items.GetType().Name.Remove(0, 3).ParseToInt(-1) select items).ToList(); 
            RSTList = RSTMgr.GetRandomList(6);
            FSTList = new List<FinalScoring>();
            FSTList.Add(new FST1());
            RBTList = (from items in RBTMgr.GetRandomList(4+3) orderby items.GetType().Name.Remove(0, 3).ParseToInt(-1) select items).ToList();
            ALTList = ALTMgr.GetList();
            AllianceTileForKnowledge = ALTList.RandomRemove();
        }
        public void GetGameView()
        {

        }
        public bool ProcessSyntax(string syntax, out string log)
        {
            log = string.Empty;
            if ("Default Game".Equals(syntax))
            {
                GameStart();
                return true;
            }
            else
            {
                log = "Syntax is wrong";
                return false;
            }
        }
        private void GameStart()
        {

        }
        /// <summary>
        /// 在游戏开始的时候实例化一个Map
        /// </summary>
        public Map Map { set; get; }
        /// <summary>
        /// 实例化四个玩家
        /// </summary>
        public List<Faction> FactionList { set; get; }
        /// <summary>
        /// 游戏的一些公共状态包括现在第几轮轮到哪个玩家行动等等
        /// </summary>
        public GameStatus GameStatus { set; get; }
        /// <summary>
        /// StandardTechList
        /// </summary>
        public List<StandardTechnology> STT6List { set; get; }
        /// <summary>
        /// StandardTechListWithNoKnowledge
        /// </summary>
        public List<StandardTechnology> STT3List { set; get; }
        /// <summary>
        /// AdvanceTechList
        /// </summary>
        public List<AdavanceTechnology> ATTList { set; get; }
        /// <summary>
        /// RoundScoringList
        /// </summary>
        public List<RoundScoring> RSTList { set; get; }
        /// <summary>
        /// FinalScoringList
        /// </summary>
        public List<FinalScoring> FSTList { set; get; }
        /// <summary>
        /// RoundBoostList
        /// </summary>
        public List<RoundBooster> RBTList { set; get; }
        /// <summary>
        /// AlianceList
        /// </summary>
        public List<AllianceTile> ALTList { set; get; }
        public AllianceTile AllianceTileForKnowledge { set; get; }


    }
}
