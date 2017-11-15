using System;
using System.Collections.Generic;
using System.Text;
using GaiaCore.Gaia.Tiles;

namespace GaiaCore.Gaia
{
    public class AC2 : GameTiles
    {
        public override string desc => "Q";
        public override bool CanAction => true;
        public override bool InvokeGameTileAction(Faction faction)
        {
            faction.QICs += 1;
            return base.InvokeGameTileAction(faction);
        }
    }
}
