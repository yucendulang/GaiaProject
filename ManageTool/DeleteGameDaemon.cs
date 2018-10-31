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
        protected override int m_timeOut { get => 300 * 1000; }

        public override void InvokeAction()
        {
            var gamelist = GameMgr.GetAllGameName();
            foreach (var item in gamelist)
            {
                GaiaGame gaiaGame = GameMgr.GetGameByName(item);
                //删除结束游戏
                if (gaiaGame.GameStatus.stage == Stage.GAMEEND)
                {
                    GameMgr.RemoveAndBackupGame(item);
                    continue;
                }

                //判断上次行动时间是否大于设置drop时间

                int hours = gaiaGame.dropHour == 0 ? 240 : gaiaGame.dropHour;
                if (DateTime.Now.AddDays(-hours / 24) > gaiaGame.LastMoveTime)
                {
                    //GameMgr.RemoveAndBackupGame(item);
                    //不需要备份直接删除
                    //GameMgr.DeleteOneGame(item);

                    //记录用户drop 次数
                    //drop
                    String userName = gaiaGame.GetCurrentUserName();
                    //如果没有用户
                    //或者时2人游戏
                    //没有用户
                    //用户数量不等
                    //第0回合
                    //一场多个用户
                    if (String.IsNullOrEmpty(userName) || gaiaGame.UserCount == 2 || gaiaGame.UserGameModels == null || gaiaGame.UserGameModels.Count != gaiaGame.UserCount || gaiaGame.GameStatus.RoundCount == 0 || gaiaGame.UserGameModels.FindAll(user => user.username == userName).Count > 1)
                    {
                        GameMgr.DeleteOneGame(item);
                        continue;
                    }
                    else
                    {
                            try
                            {
                                gaiaGame.UserGameModels.Single(user => user.username == userName).dropType = 1;
                                //阶段性操作
                                switch (gaiaGame.GameStatus.stage)
                                {
                                    case Stage.ROUNDSTART:
                                        Faction faction = gaiaGame.FactionList.SingleOrDefault(fac => fac.UserName == userName);
                                        //强制pass
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

                                //更新操作时间，防止误跳过
                                gaiaGame.LastMoveTime = DateTime.Now;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                //throw;
                            }
                        
                       
                    }
                    
                    

                }
            }
        }
    }
}

