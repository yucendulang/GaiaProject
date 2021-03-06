﻿using System;
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
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string name { get; set; }

        public abstract string desc { get; }

        /// <summary>
        /// 板块类型
        /// </summary>
        public string typename { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int showRank { get; set; }

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
        public virtual bool UndoGameTileAction(Faction faction)
        {
            IsUsed = false;
            return true;
        }

        public virtual bool PredicateGameTileAction(Faction faction) { return !IsUsed; }
        public virtual bool OneTimeAction(Faction faction) { return true; }
        public virtual bool CanAction { get => false; }
        public virtual bool IsUsed { set; get; }
        public virtual int GetTriggerScore { get; }

    }

}
