using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class LogEntity
    {
        public int Row { set; get; }
        public string Syntax { set; get; }
        public FactionBackup ResouceChange { set; get; }
        public FactionName? FactionName{set;get;}
        public FactionBackup ResouceEnd { set; get; }
        public override string ToString()
        {
            string str = string.Empty;
            if (ResouceChange != null)
            {
                str = string.Join("|", new List<string>{
                ResouceChange.m_credit.ToString().PadLeft(4,' '),
                ResouceChange.m_ore.ToString().PadLeft(4,' '),
                ResouceChange.m_QICs.ToString().PadLeft(4,' '),
                ResouceChange.m_knowledge.ToString().PadLeft(4,' '),
                ResouceChange.m_powerToken1.ToString().PadLeft(4,' '),
                ResouceChange.m_powerToken2.ToString().PadLeft(4,' '),
                ResouceChange.m_powerToken3.ToString().PadLeft(4,' '),
                ResouceChange.m_powerTokenGaia.ToString().PadLeft(4,' '),
            });
            }
            return str.Replace(" ", ".");
        }
    }
}
