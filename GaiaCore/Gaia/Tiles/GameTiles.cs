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
        public virtual int GetTurnEndScore(Faction faction) { return 0; }

        public virtual bool InvokeGameTileAction(Faction faction)
        {

            IsUsed = true;
            return true;
        }

        public virtual bool PredicateGameTileAction(Faction faction) { return !IsUsed; }
        public virtual bool OneTimeAction(Faction faction) { return true; }
        public virtual bool CanAction { get => false; }
        public virtual bool IsUsed { set; get; }
        public virtual int GetTriggerScore { get; }

    }

}
