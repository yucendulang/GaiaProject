using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia.Tiles
{
    public abstract class GameTiles
    {
        public GameTiles()
        {
            IsUsed = false;
        }
        public abstract string desc { get; }

        public virtual int GetOreIncome() { return 0; }

        public virtual int GetCreditIncome() { return 0; }

        public virtual int GetKnowledgeIncome() { return 0; }

        public virtual int GetPowerIncome() { return 0; }

        public virtual int GetQICIncome() { return 0; }

        public virtual int GetPowerTokenIncome() { return 0; }

        public virtual bool InvokeGameTileAction(Faction faction) { throw new NotImplementedException(); }
        public virtual bool PredicateGameTileAction() { return !IsUsed; }
        public virtual bool CanAction { get => false; }
        public virtual bool IsUsed { set; get; }

    }

}
