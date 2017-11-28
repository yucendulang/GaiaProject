using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    /// <summary>
    /// 专为两人计分时中立玩家写的类
    /// </summary>
    public class VirtualPlayerFaction : Faction
    {
        public VirtualPlayerFaction(FactionName name, GaiaGame gg) : base(name, gg)
        {
        }

        public override Terrain OGTerrain => Terrain.Empty;

        public override int GetBuildCount()
        {
            return 11;
        }

        public override int GetAllianceBuilding()
        {
            return 10;
        }

        public override int GetPlanetTypeCount()
        {
            return 5;
        }

        public override int GaiaPlanetNumber { get => 4; set => base.GaiaPlanetNumber = value; }
        public override int GetSpaceSectorCount()
        {
            return 6;
        }

        public override int GetSatelliteCount()
        {
            return 8;
        }
    }
}
