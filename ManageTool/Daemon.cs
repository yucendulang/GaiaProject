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
    public abstract class Daemon
    {
        protected const int STOPPED = 0;
        protected const int RUNNING = 1;
        protected string CLASS_NAME { get; }
        /// <summary>
        /// 功能是否打开
        /// </summary>
        protected int m_running = STOPPED;
        /// <summary>
        /// 执行插入的resetevent
        /// </summary>
        protected AutoResetEvent m_waitHandle = null;
        protected abstract int m_timeOut { get; }


        public Daemon()
        {
            m_waitHandle = new AutoResetEvent(false);
        }

        public void Start()
        {
            //if this task is still running, don't run it again
            if (Interlocked.CompareExchange(ref m_running, RUNNING, STOPPED) != STOPPED)
            {
                return;
            }
            Task.Factory.StartNew(Run);
        }

        protected void Run()
        {
            while (ShouldRun())
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("守护进程运行一次");
                    InvokeAction();
                    m_waitHandle.WaitOne(m_timeOut);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
        }

        public abstract void InvokeAction();

        protected bool ShouldRun()
        {
            return m_running == RUNNING;
        }

        public void Stop()
        {
            //if this task is already stopped, don't stop it again
            if (Interlocked.CompareExchange(ref m_running, STOPPED, RUNNING) != RUNNING)
            {
                return;
            }
            m_waitHandle.Set();
        }
    }
}

