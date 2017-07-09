using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaProject2.Gaia
{
    public static class MapMgr
    {
        static List<SpaceSector> ssl;
        static MapMgr()
        {
            ssl = new List<SpaceSector>();
            ssl.AddRange(BuildSpaceSector());
        }

        private static List<SpaceSector> BuildSpaceSector()
        {
            var ss1 = new List<Terrain>()
            {
                Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Brown,Terrain.Blue,
                Terrain.Yellow,Terrain.Empty,Terrain.Purple,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Orange,
                Terrain.Red
            };
            var ss2 = new List<Terrain>()
            {
                Terrain.Black,
                Terrain.Orange,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.White,
                Terrain.Empty,Terrain.Empty,Terrain.Yellow,
                Terrain.Brown,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Red,Terrain.Purple,
                Terrain.Empty
            };
            var ss3 = new List<Terrain>()
            {
                Terrain.Purple,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Green,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Black,
                Terrain.Empty,Terrain.White,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Blue,Terrain.Empty,
                Terrain.Yellow
            };
            var ss4 = new List<Terrain>()
            {
                Terrain.Black,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Red,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.White,Terrain.Empty,Terrain.Empty,
                Terrain.Orange,Terrain.Brown,
                Terrain.Empty,Terrain.Empty,Terrain.Blue,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty
            };
            var ss5 = new List<Terrain>()
            {
                Terrain.White,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Purple,
                Terrain.Green,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Red,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Orange,Terrain.Empty,
                Terrain.Yellow
            };
            var ss6 = new List<Terrain>()
            {
              Terrain.Empty,
                Terrain.Empty,Terrain.Purple,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Brown,Terrain.Blue,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Green,Terrain.Yellow,
                Terrain.Empty,Terrain.Purple,
                Terrain.Empty
            };
            var ss7 = new List<Terrain>()
            {
                Terrain.Empty,
                Terrain.Empty,Terrain.Brown,
                Terrain.Purple,Terrain.Red,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Green,Terrain.Green,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.Black
            };
            var ss8 = new List<Terrain>()
            {
                Terrain.Blue,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.White,Terrain.Purple,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Orange,Terrain.Black,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Purple,Terrain.Empty,
                Terrain.Empty
            };
            var ss9 = new List<Terrain>()
            {
                Terrain.Empty,
                Terrain.Orange,Terrain.Purple,
                Terrain.Empty,Terrain.Empty,Terrain.White,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Black,Terrain.Green,
                Terrain.Brown,Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty
            };
            var ss10 = new List<Terrain>()
            {
                Terrain.Empty,
                Terrain.Empty,Terrain.Purple,
                Terrain.Empty,Terrain.Empty,Terrain.Purple,
                Terrain.Yellow,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Green,
                Terrain.Blue,Terrain.Empty,Terrain.Empty,
                Terrain.Red,Terrain.Empty,
                Terrain.Empty
            };
            List<List<Terrain>> terrList = new List<List<Terrain>>()
            { ss1,ss2,ss3,ss4,ss5,ss6,ss7,ss8,ss9,ss10};
            List<List<TerrenHex>> hexList = new List<List<TerrenHex>>();
            foreach (var item in terrList)
            {
                hexList.Add(item.Select(y => new TerrenHex(y)).ToList());
            }
            return hexList.Select(x => new SpaceSector(x)).ToList();
        }
        public static Map GetRandomMap()
        {
            var result = new Map();
            result.AddSpaceSector(3, 10, ssl[0]);
            result.AddSpaceSector(6, 7, ssl[1]);
            result.AddSpaceSector(7, 12, ssl[2]);
            result.AddSpaceSector(10, 9, ssl[3]);
            result.AddSpaceSector(6, 2, ssl[4]);
            result.AddSpaceSector(7, 17, ssl[5]);
            result.AddSpaceSector(2, 5, ssl[6]);
            result.AddSpaceSector(3, 15, ssl[7]);
            result.AddSpaceSector(10, 4, ssl[8]);
            result.AddSpaceSector(11, 14, ssl[9]);
            return result;
        }
    }

    public class Map
    {
        public TerrenHex[,] HexArray = new TerrenHex[20,20];
        public void AddSpaceSector(int x, int y, SpaceSector ss)
        {
            var i = y % 2 == 0 ? 0 : 1;
            HexArray[x - 2, y] = ss.TerranHexArray[0];
            HexArray[x - 2 + i, y - 1] = ss.TerranHexArray[1];
            HexArray[x - 2 + i, y + 1] = ss.TerranHexArray[2];
            HexArray[x - 1, y - 2] = ss.TerranHexArray[3];
            HexArray[x - 1, y] = ss.TerranHexArray[4];
            HexArray[x - 1, y + 2] = ss.TerranHexArray[5];
            HexArray[x - 1 + i, y - 1] = ss.TerranHexArray[6];
            HexArray[x - 1 + i, y + 1] = ss.TerranHexArray[7];
            HexArray[x, y - 2] = ss.TerranHexArray[8];
            HexArray[x, y] = ss.TerranHexArray[9];
            HexArray[x, y + 2] = ss.TerranHexArray[10];
            HexArray[x + i, y - 1] = ss.TerranHexArray[11];
            HexArray[x + i, y + 1] = ss.TerranHexArray[12];
            HexArray[x + 1, y - 2] = ss.TerranHexArray[13];
            HexArray[x + 1, y] = ss.TerranHexArray[14];
            HexArray[x + 1, y + 2] = ss.TerranHexArray[15];
            HexArray[x + 1 + i, y - 1] = ss.TerranHexArray[16];
            HexArray[x + 1 + i, y + 1] = ss.TerranHexArray[17];
            HexArray[x + 2, y] = ss.TerranHexArray[18];
        }
    }
}
