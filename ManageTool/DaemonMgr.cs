using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ManageTool
{

    public static class DaemonMgr
    {
        static Daemon DeleteGameMgr = new DeleteGameDaemon();
        static Daemon BackupGameDaemon = new BackupAllGame();

        public static void StartAll()
        {
            //删除结束和过时游戏
            DeleteGameMgr.Start();
            BackupGameDaemon.Start();
        }
    }
}

