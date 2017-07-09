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
            ATTList = ATTMgr.GetRandomList(6);
            STT6List = STTMgr.GetRandomList(6);
            STT3List = STTMgr.GetOtherList(STT6List);
            RSTList = RSTMgr.GetRandomList(6);
            FSTList = new List<FinalScoring>();
            FSTList.Add(new FST1());
            RBTList = RBTMgr.GetRandomList(4+3);
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
