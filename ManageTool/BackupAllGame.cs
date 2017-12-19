using GaiaCore.Gaia;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GaiaProject.Data;
using Microsoft.EntityFrameworkCore;

namespace ManageTool
{
    /// <summary>
    /// 守护进程父类
    /// </summary>
    public class BackupAllGame : Daemon
    {
        protected override int m_timeOut { get => 300 * 1000; }

        public override void InvokeAction()
        {
            //备份到数据库
            //DbContextOptions<ApplicationDbContext> dbContextOptions=new DbContextOptions<ApplicationDbContext>();
            //ApplicationDbContext dbContext=new ApplicationDbContext(options: dbContextOptions);

            //备份到文件
            GameMgr.BackupDictionary();
            GameMgr.RemoveOldBackupData();
        }
    }
}

