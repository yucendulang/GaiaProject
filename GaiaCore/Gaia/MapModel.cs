using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaProject2.Gaia
{
    class MapModel
    {
        
    }

    /// <summary>
    /// 代表一个最小单元地块 包括了地形 归属 属于哪个Space Sector
    /// </summary>
    public class TerrenHex
    {
        public TerrenHex(Terrain t)
        {
            OGTerrain = t;
            TFTerrain = t;
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
        /// 属于哪个大板块
        /// </summary>
        public SpaceSector SpaceSector { set; get; }
        /// <summary>
        /// 属于哪个种族
        /// </summary>
        public Faction FactionBelongTo { set; get; }
        /// <summary>
        /// 对外展示的坐标名
        /// </summary>
        public string Name { set; get; }
    }
    /// <summary>
    /// Space Sector 含义参照说明书 共计十块
    /// </summary>
    public class SpaceSector
    {
        public SpaceSector(List<TerrenHex> TerranHexArray)
        {
            if (TerranHexArray.Count != 19)
            {
                throw new Exception("构造函数Hex数量不对");
            }
            if(TerranHexArray.Exists(x => x.OGTerrain == Terrain.NA))
            {
                throw new Exception("存在未初始化的地形");
            }
            this.TerranHexArray = TerranHexArray;           
        }
        public List<TerrenHex> TerranHexArray { get; }
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
