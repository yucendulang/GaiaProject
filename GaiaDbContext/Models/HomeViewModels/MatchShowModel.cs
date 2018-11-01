using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaDbContext.Models.HomeViewModels
{
    /// <summary>
    /// 需要显示的比赛详细信息
    /// </summary>
    public class MatchShowModel
    {
        public MatchInfoModel MatchInfoModel { get; set; }

        public IQueryable<MatchJoinModel> MatchJoinModels { get; set; }

        public IQueryable<GameInfoModel> GameInfoModels { get; set; }
    }
}
