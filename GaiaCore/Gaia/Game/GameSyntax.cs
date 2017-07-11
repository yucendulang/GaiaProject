using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GaiaCore.Gaia
{
    public static class GameSyntax
    {
        public const string setupGame = "SetupGame Seed";
        public  static Regex setupGameRegex = new Regex(setupGame + "[0-9]+");
    }
}
