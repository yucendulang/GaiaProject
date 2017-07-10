using GaiaCore.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaCore.Gaia;

namespace GaiaCore.Gaia
{
    class MapModel
    {
        
    }

    /// <summary>
    /// 代表一个最小单元地块 包括了地形 归属 属于哪个Space Sector
    /// </summary>
    public class TerrenHex
    {
        public TerrenHex(Terrain t,bool isCenter=false)
        {
            OGTerrain = t;
            TFTerrain = t;
            IsCenter = isCenter;
        }

        /// <summary>
        /// Origin的地形
        /// </summary>
        public Terrain OGTerrain { set; get; }
        /// <summary>
        /// Transformation的地形
        /// </summary>
        public Terrain TFTerrain { set; get; }
        /// <summary>
        /// 属于哪个大板块的名字
        /// </summary>
        public string SpaceSectorName { set; get; }
        /// <summary>
        /// 属于哪个种族
        /// </summary>
        public Faction FactionBelongTo { set; get; }
        /// <summary>
        /// 对外展示的坐标名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 是否是SpaceSector的中心点
        /// </summary>
        public bool IsCenter { set; get; }
    }
    /// <summary>
    /// Space Sector 含义参照说明书 共计十块
    /// </summary>
    public class SpaceSector
    {
        public SpaceSector(List<TerrenHex> terranHexArray)
        {
            if (terranHexArray.Count != 19)
            {
                throw new Exception("构造函数Hex数量不对");
            }
            if(terranHexArray.Exists(x => x.OGTerrain == Terrain.NA))
            {
                throw new Exception("存在未初始化的地形");
            }
            TerranHexArray = terranHexArray;
            TerranHexArray[9].IsCenter = true;
            
        }
        public List<TerrenHex> TerranHexArray { set; get; }
        /// <summary>
        /// SpaceSector的名字
        /// </summary>
        public string Name { set; get; }

        public  SpaceSector Rotate()
        {
            var newTHA = new List<TerrenHex>();
            newTHA.Add(TerranHexArray[3]);
            newTHA.Add(TerranHexArray[8]);
            newTHA.Add(TerranHexArray[1]);
            newTHA.Add(TerranHexArray[13]);
            newTHA.Add(TerranHexArray[6]);
            newTHA.Add(TerranHexArray[0]);
            newTHA.Add(TerranHexArray[11]);
            newTHA.Add(TerranHexArray[4]);
            newTHA.Add(TerranHexArray[16]);
            newTHA.Add(TerranHexArray[9]);
            newTHA.Add(TerranHexArray[2]);
            newTHA.Add(TerranHexArray[14]);
            newTHA.Add(TerranHexArray[7]);
            newTHA.Add(TerranHexArray[18]);
            newTHA.Add(TerranHexArray[12]);
            newTHA.Add(TerranHexArray[5]);
            newTHA.Add(TerranHexArray[17]);
            newTHA.Add(TerranHexArray[10]);
            newTHA.Add(TerranHexArray[15]);
            TerranHexArray = newTHA;
            return this;
        }

        public SpaceSector RandomRotato()
        {
            for (int i = 0; i < RandomInstance.Next(6); i++)
            {
                Rotate();
            }
            return this;
        }
    }

    public enum Terrain
    {
        NA=-1,
        Blue=0,
        Red,
        Orange,
        Yellow,
        Brown,
        Black,
        White,
        Green=100,
        Purple=200,
        Empty=300,
    }
}
