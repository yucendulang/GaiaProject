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
    public class BackupAllGame : Daemon
    {
        protected override int m_timeOut { get => 300 * 1000; }

        internal override void InvokeAction()
        {
            GameMgr.BackupDictionary();
            GameMgr.RemoveOldBackupData();
        }
    }
}

