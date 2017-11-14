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

        /// <summary>
        /// 4个玩家 0-3
        /// </summary>
        public int PlayerIndex { get => m_PlayerIndex - 1; set => m_PlayerIndex = value + 1; }
        /// <summary>
        /// 玩家人数
        /// </summary>
        public int PlayerNumber { get => m_PlayerNumber; set => m_PlayerNumber = value; }
        public int RoundCount { get => m_RoundCount; set => m_RoundCount = value; }
        public int TurnCount { get => m_TurnCount; set => m_TurnCount = value; }
        /// <summary>
        /// 储存Gaia阶段的行动先后顺序
        /// </summary>
        public Queue<int> GaiaPlayerIndexQueue { get; internal set; }

        public void SetPlayerIndexLast()
        {
            m_PlayerIndex = PlayerNumber;
        }

        public void NewRoundReset()
        {
            m_PlayerIndex = 1;
            m_PassPlayerIndex = new List<int>();
            RoundCount++;
            TurnCount = 1;
        }

        public void SetPassPlayerIndex(int v)
        {
            m_PassPlayerIndex.Add(v);
        }

        public bool IsAllPass()
        {
            return m_PassPlayerIndex.Count == m_PlayerNumber;
        }

        public void NextPlayer()
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

            if (m_PassPlayerIndex.Contains(PlayerIndex))
            {
                NextPlayer();
            }
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

            return m_IntialFlag && m_PlayerIndex == 0;
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
        GAMEEND
    }
}