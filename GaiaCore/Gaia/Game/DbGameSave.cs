using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaiaDbContext.Models.HomeViewModels;

namespace GaiaCore.Gaia.Game
{
    public class DbGameSave
    {
        /// <summary>
        /// 保存哪区科技版
        /// </summary>
        /// <param name="gaiaGame"></param>
        /// <param name="faction"></param>
        /// <param name="techTileStr"></param>
        public static void SaveTTData(GaiaGame gaiaGame,Faction faction,string techTileStr)
        {
            try
            {
                GameFactionExtendModel extendModel = gaiaGame.dbContext.GameFactionExtendModel.SingleOrDefault(extend => extend.gameinfo_name == gaiaGame.GameName && extend.FactionName == faction.FactionName.ToString());

                Action<GameFactionExtendModel> fz = (gameFactionExtendModel) =>
                {
                    Int16 round = (Int16)gaiaGame.GameStatus.RoundCount;
                    //回合
                    switch (techTileStr)
                    {
                        case "stt1":
                            gameFactionExtendModel.STT1 = round;
                            break;
                        case "stt2":
                            gameFactionExtendModel.STT2 = round;
                            break;
                        case "stt3":
                            gameFactionExtendModel.STT3 = round;
                            break;
                        case "stt4":
                            gameFactionExtendModel.STT4 = round;
                            break;
                        case "stt5":
                            gameFactionExtendModel.STT5 = round;
                            break;
                        case "stt6":
                            gameFactionExtendModel.STT6 = round;
                            break;
                        case "stt7":
                            gameFactionExtendModel.STT7 = round;
                            break;
                        case "stt8":
                            gameFactionExtendModel.STT8 = round;
                            break;
                        case "stt9":
                            gameFactionExtendModel.STT9 = round;
                            break;
                        //高级
                        case "att1":
                            gameFactionExtendModel.ATT1 = round;
                            break;
                        case "att2":
                            gameFactionExtendModel.ATT2 = round;
                            break;
                        case "att3":
                            gameFactionExtendModel.ATT3 = round;
                            break;
                        case "att4":
                            gameFactionExtendModel.ATT4 = round;
                            break;
                        case "att5":
                            gameFactionExtendModel.ATT5 = round;
                            break;
                        case "att6":
                            gameFactionExtendModel.ATT6 = round;
                            break;
                        case "att7":
                            gameFactionExtendModel.ATT7 = round;
                            break;
                        case "att8":
                            gameFactionExtendModel.ATT8 = round;
                            break;
                        case "att9":
                            gameFactionExtendModel.ATT9 = round;
                            break;
                        case "att10":
                            gameFactionExtendModel.ATT10 = round;
                            break;
                        case "att11":
                            gameFactionExtendModel.ATT11 = round;
                            break;
                        case "att12":
                            gameFactionExtendModel.ATT12 = round;
                            break;
                        case "att13":
                            gameFactionExtendModel.ATT13 = round;
                            break;
                        case "att14":
                            gameFactionExtendModel.ATT14 = round;
                            break;
                        case "att15":
                            gameFactionExtendModel.ATT15 = round;
                            break;
                    }
                };
                //如果没有记录
                if (extendModel == null)
                {
                    extendModel = new GameFactionExtendModel()
                    {
                        gameinfo_name = gaiaGame.GameName,//名称
                        FactionName = faction.FactionName.ToString(),//种族                        
                    };
                    fz(extendModel);
                    gaiaGame.dbContext.GameFactionExtendModel.Add(extendModel);
                }
                else
                {
                    fz(extendModel);
                    gaiaGame.dbContext.GameFactionExtendModel.Update(extendModel);
                }
                gaiaGame.dbContext.SaveChanges();

                //int? id = this.dbContext.GameFactionModel.SingleOrDefault(fac => fac.gameinfo_name == this.GameName&&fac.FactionName==faction.FactionName.ToString())?.Id;
                //int? id1 = id;
            }
            catch (Exception e)
            {

            }
        }
    }
}
