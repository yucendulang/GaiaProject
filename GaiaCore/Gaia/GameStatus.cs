namespace GaiaCore.Gaia
{
    public class GameStatus
    {
        public GameStatus()
        {
            m_PlayerIndex = 1;
            m_IntialFlag = false;
        }
        public Status status = Status.PREPARING;
        public Stage stage = Stage.RANDOMSETUP;
        public const int m_PlayerNumber = 4;
        /// <summary>
        /// 当前玩家的标签
        /// </summary>
        private int m_PlayerIndex;
        /// <summary>
        /// 初始设置房子是否走完一轮
        /// </summary>
        public bool m_IntialFlag;

        public int PlayerIndex { get => m_PlayerIndex - 1; }
        public void SetPlayerIndexLast()
        {
            m_PlayerIndex = m_PlayerNumber;
        }

        public void SetPlayerIndexFirst()
        {
            m_PlayerIndex = 1;
        }

        public void NextPlayer()
        {
            m_PlayerIndex++;
            if (m_PlayerIndex == m_PlayerNumber + 1)
            {
                m_PlayerIndex = 1;
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
                m_PlayerIndex = m_PlayerNumber;
            }
        }
        /// <summary>
        /// 所有人都选完返回0
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
                    if (m_PlayerIndex == m_PlayerNumber + 1)
                    {
                        m_IntialFlag = true;
                        m_PlayerIndex = m_PlayerNumber;
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
        ROUNDSTART
    }
}