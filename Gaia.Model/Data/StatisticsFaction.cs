// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaiaProject.Models.Data
{
    public partial class GameInfoController
    {
        public class StatisticsFaction
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string ChineseName { get; set; }
            /// <summary>
            /// 局数
            /// </summary>
            public int count { get; set; }
            /// <summary>
            /// 最低分
            /// </summary>
            public int scoremin { get; set; }

            /// <summary>
            /// 最高分
            /// </summary>
            public int scoremax { get; set; }

            public string scoremaxuser { get; set; }
            /// <summary>
            /// 平均分
            /// </summary>
            public int scoreavg { get; set; }
            /// <summary>
            /// 胜率
            /// </summary>
            public int winprobability { get; set; }
            /// <summary>
            /// 胜利场次
            /// </summary>
            public int numberwin { get; set; }

            /// <summary>
            /// 出场率
            /// </summary>
            public int OccurrenceRate { get; set; }

        }

    }
}
