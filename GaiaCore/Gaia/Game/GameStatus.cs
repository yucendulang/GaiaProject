using System.Collections.Generic;

namespace GaiaCore.Gaia
{
    public class GameStatus
    {
        public GameStatus()
        {
            m_PlayerIndex = 1;
            m_IntialFlag = false;
            m_PassPlayerIndex = new List<int>();
            GaiaPlayerIndexQueue = new Queue<int>();
            IncomePhaseIndexQueue = new Queue<int>();
        }
        public Status status = Status.PREPARING;
        public Stage stage = Stage.RANDOMSETUP;
        private int m_PlayerNumber = 4;
        private List<int> m_PassPlayerIndex;
        private int m_RoundCount = 0;
        private int m_TurnCount = 0;

        /// <summary>
        /// 当前玩家的标签
        /// </summary>
        private int m_PlayerIndex;
        /// <summary>
        /// 初始设置房子是否走完一轮
        /// </summary>
        public bool m_IntialFlag;
        private bool m_IntialFinish = false;

        /// <summary>
        /// 4个玩家 0-3
        /// </summary>
        public int PlayerIndex
        {
            get => m_PlayerIndex - 1;
            set
            {
                m_PlayerIndex = value + 1;
                //
            }
        }
        /// <summary>
        /// 玩家人数
        /// </summary>
        public int PlayerNumber { get => m_PlayerNumber; set => m_PlayerNumber = value; }
        public int RoundCount { get => m_RoundCount; set => m_RoundCount = value; }
        public int TurnCount { get => m_TurnCount; set => m_TurnCount = value; }
        /// <summary>
        /// 储存Gaia阶段的行动先后顺序
        /// </summary>
        public Queue<int> GaiaPlayerIndexQueue { get;  set; }
        public Queue<int> IncomePhaseIndexQueue { get; set; }

        public void SetPlayerIndexLast()
        {
            m_PlayerIndex = PlayerNumber;
        }

        public void SetPlayerIndex(GaiaGame gaiaGame)
        {
            //行动玩家需要是没有drop的玩家
            int index = gaiaGame.FactionList.FindIndex(item => item.UserGameModel.dropType == 0);
            m_PlayerIndex = index + 1;
        }

        /// <summary>
        /// 新回合设置
        /// </summary>
        public void NewRoundReset(GaiaGame gaiaGame)
        {

            //行动玩家需要是没有drop的玩家
            int index = gaiaGame.FactionList.FindIndex(item => item.UserGameModel.dropType == 0);

            m_PlayerIndex = index+1;
            m_PassPlayerIndex = new List<int>();
            RoundCount++;
            TurnCount = 1;

            //将drop玩家直接pass
            gaiaGame.FactionList.FindAll(item => item.UserGameModel.dropType > 0).ForEach(item =>
            {
                gaiaGame.FactionNextTurnList.Add(item);
                gaiaGame.GameStatus.SetPassPlayerIndex(gaiaGame.FactionList.IndexOf(item));
            });
        }

        public void SetPassPlayerIndex(int v)
        {
            m_PassPlayerIndex.Add(v);
        }

        public bool IsAllPass()
        {
            return m_PassPlayerIndex.Count == m_PlayerNumber;
        }
        /// <summary>
        /// 跳到下一位玩家的索引
        /// </summary>
        public void NextPlayer(List<Faction> listFactions)
        {
            if (m_PassPlayerIndex.Count == m_PlayerNumber)
            {
                throw new System.Exception("所有玩家已经Pass,不应该调用NextPlayer");
            }

            m_PlayerIndex++;
            if (m_PlayerIndex == PlayerNumber + 1)
            {
                TurnCount++;
                m_PlayerIndex = 1;
            }
            //已经pass的玩家索引跳过
            if (m_PassPlayerIndex.Contains(PlayerIndex))
            {
                NextPlayer(listFactions);
            }
            //已经drop的玩家跳过.数量不相等很可能在选族阶段
            //不需要，drop玩家强制pass
//            else if (listFactions.Count > m_PlayerIndex && listFactions[PlayerIndex].UserGameModel.dropType > 0)
//            {
//                NextPlayer(listFactions);
//            }
            //if (this.pl)
        }
        /// <summary>
        /// 倒叙选择
        /// </summary>
        public void NextPlayerReverse()
        {
            m_PlayerIndex--;
            if (m_PlayerIndex == 0)
            {
                m_PlayerIndex = PlayerNumber;
            }
        }
        /// <summary>
        /// 所有人都选完返回True 不包括特殊的Xenos
        /// </summary>
        /// <returns></returns>
        public bool NextPlayerForIntial()
        {
            if (m_IntialFinish)
            {
                return true;
            }
            if (m_IntialFlag == true)
            {
                m_PlayerIndex--;
            }
            else
            {
                m_PlayerIndex++;
                if (m_PlayerIndex == PlayerNumber + 1)
                {
                    m_IntialFlag = true;
                    m_PlayerIndex = PlayerNumber;
                }
            }
            if (m_PassPlayerIndex.Contains(PlayerIndex))
            {
                NextPlayerForIntial();
            }
            if(m_IntialFlag && m_PlayerIndex == 0)
            {
                m_IntialFinish = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public enum Status
    {
        PREPARING=0,
        RUNNING,
        ABORTED,
        ENDED
    }

    public enum Stage
    {
        RANDOMSETUP,
        FACTIONSELECTION,
        INITIALMINES,
        SELECTROUNDBOOSTER,
        ROUNDINCOME,
        ROUNDSTART,
        ROUNDWAITLEECHPOWER,
        ROUNDGAIAPHASE,
        GAMEEND,
        MAPROTATE,//旋转地图
    }
}