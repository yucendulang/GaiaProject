using System;
using System.Collections.Generic;
using System.Text;
using GaiaCore.Gaia.Tiles;

namespace GaiaCore.Gaia
{
    public class AC2 : GameTiles
    {
        public AC2()
        {
            this.typename = "ac";
            this.showRank = 10;
        }
        public override string desc => "Q";
        public override bool CanAction => true;
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.QICs += 1;
            return base.InvokeGameTileAction(faction);
        }
    }
}
