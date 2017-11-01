using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaCore.Util;

namespace GaiaCore.Gaia
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
            var result = hexList.Select(x => new SpaceSector(x)).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Name = i.ToString("D2");
                result[i].TerranHexArray.ForEach(x => x.SpaceSectorName = result[i].Name);
            }

            return result;
        }
        public static Map GetRandomMap(Random random)
        {
            var result = new Map();
            result.AddSpaceSector(3, 10, ssl[0],random);
            result.AddSpaceSector(6, 7, ssl[1], random);
            result.AddSpaceSector(7, 12, ssl[2], random);
            result.AddSpaceSector(10, 9, ssl[3], random);
            var randomList = new List<SpaceSector>()
            {
                ssl[4],ssl[5],ssl[6],ssl[7],ssl[8],ssl[9]
            };
            var centerTuple = new List<Tuple<int, int>>()
            {
                { 6,2},{ 7,17},{2,5},{3,15},{10,4},{11,14 }
            };

            centerTuple.ForEach(x =>
            {
                var index = random.Next(randomList.Count);
                System.Diagnostics.Debug.WriteLine("index is "+index);
                result.AddSpaceSector(x.Item1, x.Item2, randomList[index].RandomRotato(random), random);
                randomList.RemoveAt(index);
            });
            System.Diagnostics.Debug.WriteLine(randomList.Count);
            return result;
        }
    }

    public class Map
    {
        public const int m_mapWidth = 20;
        public const int m_mapHeight = 20;
        public TerrenHex[,] HexArray = new TerrenHex[m_mapWidth, m_mapHeight];
        public void AddSpaceSector(int x, int y, SpaceSector ss,Random random)
        {

            List<Tuple<int, int>> hexList = GetHexList(x, y);
            for (int j = 0; j < ss.TerranHexArray.Count; j++)
            {
                HexArray[hexList[j].Item1, hexList[j].Item2] = ss.TerranHexArray[j];
            }
            if (!ValidateMap(hexList))
            {
                //System.Diagnostics.Debug.WriteLine("发现不合法");
                AddSpaceSector(x, y, ss.RandomRotato(random),random);
            }
        }

        private bool ValidateMap(List<Tuple<int, int>> hexList)
        {
            foreach (var item in hexList)
            {
                if (!IsOneHexValidate(item.Item1, item.Item2))
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("发现不合法{0} {1}", item.Item1, item.Item2));
                    return false;
                }
            }
            return true;
        }

        private bool IsOneHexValidate(int item1, int item2)
        {
            if (HexArray[item1, item2] == null)
            {
                return false;
            }
            if (HexArray[item1, item2].OGTerrain == Terrain.Empty)
            {
                return true;
            }
            var i = item2 % 2 == 0 ? 0 : 1;
            if (IsTwoHexColorSame(item1 - 1, item2, item1, item2))
            {
                return false;
            }
            if (IsTwoHexColorSame(item1 - 1 + i, item2 + 1, item1, item2))
            {
                return false;
            }
            if (IsTwoHexColorSame(item1 - 1 + i, item2 - 1, item1, item2))
            {
                return false;
            }
            if (IsTwoHexColorSame(item1 + i, item2 + 1, item1, item2))
            {
                return false;
            }
            if (IsTwoHexColorSame(item1 + i, item2 - 1, item1, item2))
            {
                return false;
            }
            if (IsTwoHexColorSame(item1 + 1, item2, item1, item2))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// x为周边点 x2为中心点保证有效
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private bool IsTwoHexColorSame(int x, int y, int x2, int y2)
        {

            if (x < 0 || y < 0||x>=m_mapWidth||y>=m_mapHeight)
                return false;
            if (HexArray[x, y] == null)
                return false;
            if (HexArray[x, y].SpaceSectorName.Equals(HexArray[x2, y2].SpaceSectorName))
                return false;
            if (HexArray[x, y].OGTerrain == HexArray[x2, y2].OGTerrain)
                return true;
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 根据中心点算出要操作的Hex块
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static List<Tuple<int, int>> GetHexList(int x, int y)
        {
            var i = y % 2 == 0 ? 0 : 1;
            return new List<Tuple<int, int>>()
            {
                {x - 2, y},
                {x - 2 +i, y - 1},
                {x - 2 +i, y + 1},
                {x - 1, y - 2},
                {x - 1, y},
                {x - 1, y + 2},
                {x - 1 + i, y - 1},
                {x - 1 + i, y + 1},
                {x, y - 2},
                {x, y},
                {x, y + 2},
                {x + i, y - 1} ,
                {x + i, y + 1} ,
                {x + 1, y - 2} ,
                {x + 1, y} ,
                {x + 1, y + 2},
                {x + 1 + i, y - 1},
                {x + 1 + i, y + 1},
                {x + 2, y},
            };
        }
        public bool CalIsBuildValidate(int x, int y, FactionName name, int distance)
        {
            for (int i = Math.Max(x - distance, 0); i < Math.Min(x + distance, m_mapHeight); i++)
            {
                for (int j = Math.Max(y - distance, 0); j < Math.Min(j + distance, m_mapWidth); j++)
                {
                    if (Math.Sqrt((i - x) * (i - x) + (j - y) * (j - y)) <= distance)
                    {
                        if (HexArray[i, j].FactionBelongTo == name)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }
    }
}