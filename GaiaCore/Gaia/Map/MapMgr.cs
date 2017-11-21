using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaCore.Util;

namespace GaiaCore.Gaia
{
    public class MapMgr
    {

        List<SpaceSector> ssl;
        public MapMgr()
        {
            ssl = new List<SpaceSector>();
            ssl.AddRange(BuildSpaceSector());
        }

        private List<SpaceSector> BuildSpaceSector()
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
                Terrain.Gray,
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
                Terrain.Empty,Terrain.Empty,Terrain.Gray,
                Terrain.Empty,Terrain.White,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Blue,Terrain.Empty,
                Terrain.Yellow
            };
            var ss4 = new List<Terrain>()
            {
                Terrain.Gray,
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
                Terrain.Gray
            };
            var ss8 = new List<Terrain>()
            {
                Terrain.Blue,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.White,Terrain.Purple,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Orange,Terrain.Gray,
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
                Terrain.Gray,Terrain.Green,
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
            var ss52 = new List<Terrain>()
            {
                Terrain.White,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Purple,
                Terrain.Green,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Red,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Orange,Terrain.Empty,
                Terrain.Empty
            };
            var ss62 = new List<Terrain>()
            {
                Terrain.Empty,
                Terrain.Empty,Terrain.Purple,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Blue,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Green,Terrain.Yellow,
                Terrain.Empty,Terrain.Purple,
                Terrain.Empty
            };
            var ss72 = new List<Terrain>()
            {
                Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.Purple,Terrain.Green,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Green,Terrain.Brown,
                Terrain.Empty,Terrain.Empty,Terrain.Empty,
                Terrain.Empty,Terrain.Empty,
                Terrain.Gray
            };
            List<List<Terrain>> terrList = new List<List<Terrain>>()
            { ss1,ss2,ss3,ss4,ss5,ss6,ss7,ss8,ss9,ss10,ss52,ss62,ss72};
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
        public Map Get4PRandomMap(Random random)
        {
            var result = new Map();
            result.AddSpaceSector(3, 10, ssl[0], random);
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
                result.AddSpaceSector(x.Item1, x.Item2, randomList[index].RandomRotato(random), random);
                randomList.RemoveAt(index);
            });
            System.Diagnostics.Debug.WriteLine(randomList.Count);
            return result;
        }

        public Map Get2PRandomMap(Random random)
        {
            var result = new Map();
            result.AddSpaceSector(6, 7, ssl[2], random);
            var randomList = new List<SpaceSector>()
            {
                ssl[0],ssl[1],ssl[3],ssl[10],ssl[11],ssl[12]
            };
            var centerTuple = new List<Tuple<int, int>>()
            {
                { 2,5},{6,2},{10,4 },{ 3,10},{7,12},{ 10,9}
            };

            centerTuple.ForEach(x =>
            {
                var index = random.Next(randomList.Count);
                result.AddSpaceSector(x.Item1, x.Item2, randomList[index].RandomRotato(random), random);
                randomList.RemoveAt(index);
            });
            System.Diagnostics.Debug.WriteLine(randomList.Count);
            return result;
        }

        public Map Get3PRandomMap(Random random)
        {
            var result = new Map();
            result.AddSpaceSector(6, 7, ssl[2], random);
            var randomList = new List<SpaceSector>()
            {
                ssl[0],ssl[1],ssl[3],ssl[4],ssl[5],ssl[6]
            };
            var centerTuple = new List<Tuple<int, int>>()
            {
                { 2,5},{6,2},{10,4 },{ 3,10},{7,12},{ 10,9}
            };

            centerTuple.ForEach(x =>
            {
                var index = random.Next(randomList.Count);
                result.AddSpaceSector(x.Item1, x.Item2, randomList[index].RandomRotato(random), random);
                randomList.RemoveAt(index);
            });
            System.Diagnostics.Debug.WriteLine(randomList.Count);
            return result;
        }


        public Map Get2PFixedMap()
        {
            var centerTuple = new List<Tuple<int, int>>()
            {
                { 2,5},{6,2},{6,7},{10,4 },{ 3,10},{7,12},{ 10,9}
            };
            var sslList = new List<int>()
            {
                0,1,2,3,10,11,12
            };
            var result = new Map();
            foreach (var item in centerTuple)
            {
                result.AddSpaceSector(item.Item1, item.Item2, ssl[sslList[centerTuple.IndexOf(item)]], null);
            }
            return result;
        }
        public Map Get3PFixedMap()
        {
            var centerTuple = new List<Tuple<int, int>>()
            {
                { 2,5},{6,2},{6,7},{10,4 },{ 3,10},{7,12},{ 10,9}
            };
            var sslList = new List<int>()
            {
                0,1,2,3,4,5,6
            };
            var result = new Map();
            foreach (var item in centerTuple)
            {
                result.AddSpaceSector(item.Item1, item.Item2, ssl[sslList[centerTuple.IndexOf(item)]], null);
            }
            return result;
        }

        public Map Get4PFixedMap()
        {
            var centerTuple = new List<Tuple<int, int>>()
            {
                { 2,5},{ 3,10},{ 3,15},{6,2},{6,7},{7,12},{ 7,17},{10,4 },{ 10,9},{11,14 }
            };
            var sslList = new List<int>()
            {
                9,0,4,8,1,2,5,7,3,6
            };
            var result = new Map();
            foreach (var item in centerTuple)
            {
                result.AddSpaceSector(item.Item1, item.Item2, ssl[sslList[centerTuple.IndexOf(item)]], null);
            }
            return result;
        }
    }

    public class Map
    {
        public const int m_mapWidth = 20;
        public const int m_mapHeight = 20;
        public TerrenHex[,] HexArray = new TerrenHex[m_mapWidth, m_mapHeight];
        public TerrenHex GetHex(int x, int y)
        {
            return HexArray[x, y];
        }
        public TerrenHex GetHex(Tuple<int, int> item)
        {
            return HexArray[item.Item1, item.Item2];
        }
        public IList<TerrenHex> GetHexList()
        {
            var list = new List<TerrenHex>();
            foreach (var item in HexArray)
            {
                if (item != null)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public List<Tuple<int, int>> GetHexListForBuildingAndSatellite(FactionName name)
        {
            var list = new List<Tuple<int, int>>();
            for (int i = 0; i < m_mapHeight; i++)
            {
                for (int j = 0; j < m_mapWidth; j++)
                {
                    if (HexArray[i, j] != null && (HexArray[i, j].FactionBelongTo == name || (HexArray[i, j].Satellite?.Contains(name)).GetValueOrDefault()))
                    {
                        list.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return list;
        }
        public void AddSpaceSector(int x, int y, SpaceSector ss, Random random)
        {

            List<Tuple<int, int>> hexList = GetHexList(x, y);
            for (int j = 0; j < ss.TerranHexArray.Count; j++)
            {
                HexArray[hexList[j].Item1, hexList[j].Item2] = ss.TerranHexArray[j];
            }
            if (random != null)
            {
                if (!ValidateMap(hexList))
                {
                    //System.Diagnostics.Debug.WriteLine("发现不合法");
                    AddSpaceSector(x, y, ss.RandomRotato(random), random);
                }
            }
        }

        private bool ValidateMap(List<Tuple<int, int>> hexList)
        {
            foreach (var item in hexList)
            {
                if (!IsOneHexValidate(item.Item1, item.Item2))
                {
                    //System.Diagnostics.Debug.WriteLine(string.Format("发现不合法{0} {1}", item.Item1, item.Item2));
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

            if (x < 0 || y < 0 || x >= m_mapWidth || y >= m_mapHeight)
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
            for (int i = Math.Max(x - distance, 0); i <= Math.Min(x + distance, m_mapHeight - 1); i++)
            {
                for (int j = Math.Max(y - distance, 0); j <= Math.Min(j + distance, m_mapWidth - 1); j++)
                {
                    if (HexArray[i, j] != null && CalTwoHexDistance(x, y, i, j) <= distance)
                    {
                        if ((HexArray[i, j].FactionBelongTo == name && !(HexArray[i, j].Building is GaiaBuilding)) ||
                            (HexArray[i, j].SpecialBuilding != null && name == FactionName.Lantida) ||
                            (HexArray[i, j].IsSpecialSatelliteForHive == true && name == FactionName.Hive))
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        public int CalHighestPowerBuilding(int x, int y, Faction faction)
        {
            //吸魔力大小范围
            var distance = 2;
            var res = 0;
            for (int i = Math.Max(x - distance, 0); i <= Math.Min(x + distance, m_mapHeight - 1); i++)
            {
                for (int j = Math.Max(y - distance, 0); j <= Math.Min(j + distance, m_mapWidth - 1); j++)
                {
                    if (CalTwoHexDistance(x, y, i, j) <= distance)
                    {
                        //System.Diagnostics.Debug.WriteLine("row:" + i + " col:" + j);

                        if (HexArray[i, j] != null && HexArray[i, j].FactionBelongTo == faction.FactionName)
                        {
                            if ((faction is MadAndroid) && faction.StrongHold == null && faction.OGTerrain == HexArray[i, j].TFTerrain)
                            {
                                res = Math.Max(res, HexArray[i, j].Building.MagicLevel + 1);
                            }
                            else
                            {
                                res = Math.Max(res, HexArray[i, j].Building.MagicLevel);
                            }
                        }
                        else if (HexArray[i, j] != null && (faction is Lantida) && HexArray[i, j].SpecialBuilding != null)
                        {
                            res = Math.Max(res, 1);
                        }
                    }

                }
            }
            return res;
        }

        public int CalTwoHexDistance(int x1, int y1, int x2, int y2)
        {
            var a1 = x1 - (int)Math.Floor(y1 / 2.0);
            var b1 = y1;
            var a2 = x2 - (int)Math.Floor(y2 / 2.0);
            var b2 = y2;
            var dx = a2 - a1;
            var dy = b2 - b1;
            var dist = Math.Max(Math.Max(Math.Abs(dx), Math.Abs(dy)), Math.Abs(dx + dy));
            return dist;
        }
        /// <summary>
        /// 寻找周围的魔力建筑 不包括GaiaBuilding
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Tuple<int, int>> GetSurroundhexWithBuild(int x, int y, FactionName name, int? dis = null)
        {
            //吸魔力大小范围
            var ret = new List<Tuple<int, int>>();
            int distance = 0;
            if (dis.HasValue)
            {
                distance = dis.Value;
            }
            else
            {
                distance = 1;
            }
            for (int i = Math.Max(x - distance, 0); i <= Math.Min(x + distance, m_mapHeight - 1); i++)
            {
                for (int j = Math.Max(y - distance, 0); j <= Math.Min(j + distance, m_mapWidth - 1); j++)
                {
                    if (CalTwoHexDistance(x, y, i, j) <= distance)
                    {
                        //System.Diagnostics.Debug.WriteLine("row:" + i + " col:" + j);

                        if ((HexArray[i, j] != null && HexArray[i, j].FactionBelongTo == name && !(HexArray[i, j].Building is GaiaBuilding)) ||
                            (name == FactionName.Lantida && HexArray[i, j] != null && HexArray[i, j].SpecialBuilding != null))
                        {
                            ret.Add(new Tuple<int, int>(i, j));
                        }
                    }

                }
            }
            return ret;
        }

        /// <summary>
        /// 寻找周围本家建筑与卫星
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="name"></param>
        /// <param name="list">传入的不会返回</param>
        /// <returns></returns>
        public List<Tuple<int, int>> GetSurroundhexWithBuildingAndSatellite(int x, int y, FactionName name, int? dis = null, List<Tuple<int, int>> list = null)
        {
            //吸魔力大小范围
            var ret = new List<Tuple<int, int>>();
            int distance = 0;
            if (dis.HasValue)
            {
                distance = dis.Value;
            }
            else
            {
                distance = 1;
            }
            for (int i = Math.Max(x - distance, 0); i <= Math.Min(x + distance, m_mapHeight - 1); i++)
            {
                for (int j = Math.Max(y - distance, 0); j <= Math.Min(j + distance, m_mapWidth - 1); j++)
                {
                    if (CalTwoHexDistance(x, y, i, j) <= distance)
                    {
                        //System.Diagnostics.Debug.WriteLine("row:" + i + " col:" + j);

                        if (HexArray[i, j] != null &&
                            ((HexArray[i, j].FactionBelongTo == name && !(HexArray[i, j].Building is GaiaBuilding) ||
                                HexArray[i, j].Satellite != null && HexArray[i, j].Satellite.Contains(name))))
                        {
                            if (list == null || (list != null && !list.Exists(z => z.Item1 == x && z.Item2 == y)))
                            {
                                ret.Add(new Tuple<int, int>(i, j));
                            }
                        }
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 寻找周围的空的能放卫星的格子
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="name"></param>
        /// <param name="dis"></param>
        /// <returns></returns>
        public List<Tuple<int, int>> GetSurroundhex(int x, int y, FactionName name, List<Tuple<int, int>> list)
        {
            //吸魔力大小范围
            var ret = new List<Tuple<int, int>>();
            int distance = 1;
            for (int i = Math.Max(x - distance, 0); i <= Math.Min(x + distance, m_mapHeight - 1); i++)
            {
                for (int j = Math.Max(y - distance, 0); j <= Math.Min(j + distance, m_mapWidth - 1); j++)
                {
                    if (CalTwoHexDistance(x, y, i, j) <= distance)
                    {
                        if (HexArray[i, j] != null && HexArray[i, j].TFTerrain == Terrain.Empty && !HexArray[i, j].Satellite.Contains(name))
                        {
                            var surroundHex = GetSurroundhexWithBuildingAndSatellite(i, j, name);
                            surroundHex.RemoveAll(z => list.Contains(z));
                            if (surroundHex.Count == 0)
                            {
                                ret.Add(new Tuple<int, int>(i, j));
                            }
                        }
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// 寻找周围的建筑 包括传入的卫星部分
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="name"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<Tuple<int, int>> GetSatellitehex(int x, int y, FactionName name, List<Tuple<int, int>> list)
        {
            //吸魔力大小范围
            var ret = new List<Tuple<int, int>>();
            var distance = 1;
            for (int i = Math.Max(x - distance, 0); i <= Math.Min(x + distance, m_mapHeight - 1); i++)
            {
                for (int j = Math.Max(y - distance, 0); j <= Math.Min(j + distance, m_mapWidth - 1); j++)
                {
                    if (CalTwoHexDistance(x, y, i, j) <= distance)
                    {
                        //System.Diagnostics.Debug.WriteLine("row:" + i + " col:" + j);

                        if ((HexArray[i, j] != null && HexArray[i, j].FactionBelongTo == name && !(HexArray[i, j].Building is GaiaBuilding))
                        || list.Exists(z => z.Item1 == i && z.Item2 == j))
                        {
                            ret.Add(new Tuple<int, int>(i, j));
                        }
                    }

                }
            }
            return ret;
        }

        public int CalShipDistanceNeed(int row, int col, FactionName factionName)
        {
            for (int i = 1; i < m_mapHeight; i++)
            {
                if (CalIsBuildValidate(row, col, factionName, i))
                {
                    return i;
                }
            }
            throw new Exception("无法结算航海距离");
        }
    }
}