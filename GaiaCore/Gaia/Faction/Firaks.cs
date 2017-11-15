using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Firaks : Faction
    {
        public Firaks(GaiaGame gg) :base(FactionName.Firaks, gg)
        {
            this.ChineseName = "章鱼人";
            this.ColorCode = colorList[5];
            this.ColorMap = colorMapList[5];

            m_knowledge -= 1;
            m_ore -= 1;
        }
        public override Terrain OGTerrain { get => Terrain.Gray; }
        public override void CalIncome()
        {
            m_knowledge += 1;
            base.CalIncome();
        }

        internal bool DowngradeBuilding(int row, int col, out string log)
        {
            log = string.Empty;
            var hex = GaiaGame.Map.HexArray[row, col];
            if (!(hex.FactionBelongTo == this.FactionName && hex.Building is ResearchLab))
            {
                log = "执行Downgrade命令必须对着自己的ResearchLab执行";
                return false;
            }
            if (!TradeCenters.Any())
            {
                log = "玩家必须还剩余TC";
                return false;
            }

            ActionQueue.Enqueue(() =>
            {
                ResearchLabs.Add(hex.Building as ResearchLab);
                hex.Building = TradeCenters.First();
                TradeCenters.RemoveAt(0);
                TriggerRST(typeof(RST2));
            });
            TechTracAdv++;
            FactionSpecialAbility--;
            return true;
        }

        protected override void CallSpecialSHBuild()
        {
            AddGameTiles(new Fir());
            base.CallSpecialSHBuild();
        }

        public class Fir : MapAction
        {
            public override string desc => "SH能力";
            protected override int ResourceCost => 0;
            public override bool CanAction => true;
            public override bool InvokeGameTileAction(Faction faction)
            {
                faction.FactionSpecialAbility++;
                faction.ActionQueue.Enqueue(() =>
                {
                    base.InvokeGameTileAction(faction);
                });
                return true;
            }
        }
    }
}
