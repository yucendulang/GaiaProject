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
                if (GameMgr.GetGameByName(item).GameStatus.stage == Stage.GAMEEND)
                {
                    GameMgr.RemoveAndBackupGame(item);
                }
                if (DateTime.Now.AddDays(-1) > GameMgr.GetGameByName(item).LastMoveTime)
                {
                    GameMgr.RemoveAndBackupGame(item);
                }
            }
        }
    }
}

