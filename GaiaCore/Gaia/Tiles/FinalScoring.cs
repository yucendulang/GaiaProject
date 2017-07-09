using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia.Tiles
{

    public abstract class FinalScoring:GameTiles
    {
    }

    public class FST1 : FinalScoring
    {
        public override string desc
        {
            get
            {
                return "network";
            }
        }
    }
}
