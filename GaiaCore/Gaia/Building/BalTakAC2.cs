using GaiaCore.Gaia.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia.BalTakBuilding
{
    public class AC2 : GameTiles
    {
        public AC2()
        {
            this.typename = "BalAC2";
        }
        public override string desc => "4C";
        public override bool CanAction => true;
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.Credit += 4;
            return base.InvokeGameTileAction(faction);
        }
    }
}
