

namespace Gaia.Model.Match
{
    /// <summary>
    /// 比赛用户统计
    /// </summary>
    public class MatchUserStatistics
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 参加次数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public int ScoreTotal { get; set; }
        /// <summary>
        /// 平均分
        /// </summary>
        public float ScoreAvg { get; set; }
    }
}