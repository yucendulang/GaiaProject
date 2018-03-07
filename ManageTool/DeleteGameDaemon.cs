using GaiaCore.Gaia;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
                }
                //删除超过4天游戏
                if (DateTime.Now.AddDays(-4) > GameMgr.GetGameByName(item).LastMoveTime)
                {
                    GameMgr.RemoveAndBackupGame(item);
                }
            }
        }
    }
}

