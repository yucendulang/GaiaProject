using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaiaCore.Gaia;
using GaiaCore.Util;
using Microsoft.AspNetCore.Mvc;

namespace GaiaProject.Controllers
{
    public class AjaxController : Controller
    {
        //        public IActionResult Index()
        //        {
        //            return View();
        //        }

        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CalShipDistanceNeed(string id,string pos, string factionName,int tempship)
        {
            GaiaGame gaiaGame = GameMgr.GetGameByName(id);
            Faction faction = gaiaGame.FactionList.Find(x => x.FactionName.ToString() == factionName);

            Models.Data.UserFriendController.JsonData jsonData = new Models.Data.UserFriendController.JsonData();

            //Faction faction=null;
            if (!string.IsNullOrEmpty(pos))
            {

                pos = pos.ToLower();

                ConvertPosToRowCol(pos, out int row, out int col);
                //距离
                int distanceNeed = gaiaGame.Map.CalShipDistanceNeed(row, col, faction.FactionName);
                distanceNeed = distanceNeed - tempship;
                //需要的Q
                int QSHIP = Math.Max((distanceNeed - faction.GetShipDistance + 1) / 2, 0);

                jsonData.info.state = 200;
                jsonData.data = QSHIP;
            }
            return new JsonResult(jsonData);
        }

        private void ConvertPosToRowCol(string position, out int row, out int col)
        {
            row = position.Substring(0, 1).ToCharArray().First() - 'a';
            col = position.Substring(1).ParseToInt(0);
        }
    }
}