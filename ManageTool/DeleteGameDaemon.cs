using GaiaCore.Gaia;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GaiaCore.Gaia.Data;

namespace ManageTool
{
    /// <summary>
    /// 守护进程父类
    /// </summary>
    public class DeleteGameDaemon : Daemon
    {
        protected override int m_timeOut { get => 3600 * 1000; }

        public override void InvokeAction()
        {
            var gamelist = GameMgr.GetAllGameName();
            foreach (var item in gamelist)
            {
                //删除结束游戏
                if (GameMgr.GetGameByName(item).GameStatus.stage == Stage.GAMEEND)
                {
                    GameMgr.RemoveAndBackupGame(item);
                    continue;
                }

                //判断上次行动时间是否大于设置drop时间
                GaiaGame gaiaGame = GameMgr.GetGameByName(item);
                if (DateTime.Now.AddDays(-gaiaGame.dropHour/24) > GameMgr.GetGameByName(item).LastMoveTime)
                {
                    //GameMgr.RemoveAndBackupGame(item);
                    //不需要备份直接删除
                    //GameMgr.DeleteOneGame(item);

                    //记录用户drop 次数
                    //drop
                    String userName = gaiaGame.GetCurrentUserName();
                    gaiaGame.UserGameModels.Single(user => user.username == userName).dropType = 1;
                    //阶段性操作
                    switch (gaiaGame.GameStatus.stage)
                    {
                        case Stage.ROUNDSTART:
                            Faction faction = gaiaGame.FactionList.SingleOrDefault(fac => fac.UserName == userName);
                            gaiaGame.FactionNextTurnList.Add(faction);
                            gaiaGame.GameStatus.SetPassPlayerIndex(gaiaGame.FactionList.IndexOf(faction));
                            gaiaGame.GameStatus.NextPlayer(gaiaGame.FactionList);
                            break;
                        case Stage.ROUNDGAIAPHASE:
                            gaiaGame.GaiaNextPlayer();
                            break;
                        case Stage.ROUNDINCOME:
                            gaiaGame.IncomePhaseNextPlayer();
                            break;
                        default:
                            //其它情况
                            break;
                    }
                   

                }
            }
        }
    }
}

