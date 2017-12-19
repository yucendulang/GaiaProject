using System;
using System.Collections.Generic;
using System.Text;

namespace ManageTool
{
    public static class ServerStatus
    {
        public static bool IsStopSyntax { get; set; }
        public static DateTime? ServerStartTime = null;
        static ServerStatus()
        {
            IsStopSyntax = false;
        }
    }
}
