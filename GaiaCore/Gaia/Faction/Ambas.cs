using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public class Ambas : Faction
    {
        public Ambas(GaiaGame gg) : base(FactionName.Ambas, gg)
        {
            this.ChineseName = "大使星人";
            this.ColorCode = colorList[4];
            this.ColorMap = colorMapList[4];
            if (gg != null)
            {
                IncreaseTech("ship");
            }
        }
        public override Terrain OGTerrain { get => Terrain.Brown; }
        public override void CalIncome()
        {
            base.CalIncome();
        }
        protected override int CalOreIncome()
        {
            return base.CalOreIncome()+1;
        }

        protected override int CallSHPowerTokenIncome()
        {
            return base.CallSHPowerTokenIncome()+1;
        }

        protected override void CallSpecialSHBuild()
        {
            AddGameTiles(new Amb());
            base.CallSpecialSHBuild();
        }

        public bool ExcuteSHAbility(Tuple<int, int> pos1, Tuple<int, int> pos2, out string log)
        {
            log = string.Empty;
            var hex1 = GaiaGame.Map.HexArray[pos1.Item1, pos1.Item2];
            var hex2 = GaiaGame.Map.HexArray[pos2.Item1, pos2.Item2];

            if (hex1.FactionBelongTo != FactionName || hex2.FactionBelongTo != FactionName)
            {
                log = "两个地点必须都属于" + ChineseName;
                return false;
            }
            if (hex1.TFTerrain == Terrain.Black || hex2.TFTerrain == Terrain.Black)
            {
                log = "死星不能交换";
                return false;
            }
            if (!((hex1.Building is StrongHold && hex2.Building is Mine)
                || (hex2.Building is StrongHold && hex1.Building is Mine)))
            {
                log = "两个建筑必须一个是矿场一个是要塞";
                return false;
            }
            ActionQueue.Enqueue(() =>
            {
                var tempbuild = hex1.Building;
                hex1.Building = hex2.Building;
                hex2.Building = tempbuild;
            });
            FactionSpecialAbility--;
            return true;
        }
        public class Amb : MapAction
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
