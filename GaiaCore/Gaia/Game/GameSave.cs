﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GaiaDbContext.Models.HomeViewModels;
using GaiaProject.Data;

namespace GaiaCore.Gaia.Game
{
    public class GameSave
    {
        /// <summary>
        /// 保存到数据库
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="gaiaGame"></param>
        public static void SaveGameToDb(ApplicationDbContext dbContext, GaiaGame gaiaGame)
        {


            //游戏结束
            bool flag = gaiaGame.GameStatus.RoundCount == GameConstNumber.GameRoundCount;

            if (flag)
            {
                //保存
                if (ApplicationDbContext.isSaveResult)
                {
                    try
                    {
                        //保存游戏结束时的信息
                        //先保存游戏信息
                        var gameinfo = dbContext.GameInfoModel.SingleOrDefault(item => item.name == gaiaGame.GameName);

                        if (gameinfo != null)
                        {
                            if (gameinfo.GameStatus == 8)
                            {
                                return;
                            }

                            gameinfo.GameStatus = 8; //状态
                            gameinfo.round = 7;//代表结束
                            gameinfo.version = gaiaGame.version; //版本
                            gameinfo.UserCount = gaiaGame.UserCount; //玩家数量
                            gameinfo.endtime = DateTime.Now; //结束时间
                            //gameinfo.ATTList = string.Join("|", ATTList.Select(item => item.name));
                            //gameinfo.FSTList = string.Join("|", FSTList.Select(item => item.GetType().Name));
                            //gameinfo.RBTList = string.Join("|", RBTList.Select(item => item.name));
                            //gameinfo.RSTList = string.Join("|", RSTList.Select(item => item.GetType().Name));
                            //gameinfo.STT3List = string.Join("|",
                            //    STT3List.GroupBy(item => item.name).Select(g => g.Max(item => item.name)));
                            //gameinfo.STT6List = string.Join("|",
                            //    STT6List.GroupBy(item => item.name).Select(g => g.Max(item => item.name)));
                            gameinfo.loginfo = string.Join("|", gaiaGame.LogEntityList.Select(item => item.Syntax)) + "|" +
                                               gaiaGame.syntax;
                            gameinfo.scoreFaction =
                                string.Join(":",
                                    gaiaGame.FactionList.OrderByDescending(item => item.Score)
                                        .Select(item => string.Format("{0}{1}({2})", item.ChineseName,
                                            item.Score, item.UserName))); //最后的得分情况

                            dbContext.GameInfoModel.Update(gameinfo);


                            //dbContext.SaveChanges();
                            //保存种族信息
                            SaveFactionToDb(dbContext, gaiaGame, gameinfo);


                            dbContext.SaveChanges();
                        }


                    }
                    catch (Exception e)
                    {
                        string msg = e.Message;
                        int a = 1;
                    }
                }
            }
        }
        /// <summary>
        /// 保存种族信息到数据库
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="gaiaGame"></param>
        /// <param name="gameInfoModel"></param>
        public static void SaveFactionToDb(ApplicationDbContext dbContext, GaiaGame gaiaGame, GameInfoModel gameInfoModel)
        {
            //再保存玩家信息
            Func<Faction, int, int> getscore = (faction, index) =>
            {
                faction.FinalEndScore = 0;
                gaiaGame.FSTList[index].InvokeGameTileAction(gaiaGame.FactionList);
                return faction.FinalEndScore;
            };
            //排名
            int rankindex = 1;
            var factionList = gaiaGame.FactionList.OrderByDescending(f => f.Score).ToList();
            //如果第一名小于100
            if (factionList[0].Score < 100)
            {

            }
            foreach (Faction faction in factionList)
            {
                GameFactionModel gameFactionModel = dbContext.GameFactionModel.SingleOrDefault(
                    item => item.gameinfo_id == gameInfoModel.Id && item.FactionName == faction.FactionName.ToString());
                //没有就新建
                bool isAdd = true;
                if (gameFactionModel == null)
                {
                    gameFactionModel = new GaiaDbContext.Models.HomeViewModels.GameFactionModel()
                    {
                        gameinfo_id = gameInfoModel.Id,
                        gameinfo_name = gaiaGame.GameName,
                        FactionName = faction.FactionName.ToString(),
                        FactionChineseName = faction.ChineseName,

                    };
                    gameFactionModel.userid = null;
                    gameFactionModel.username = faction.UserName;
                    gameFactionModel.scoreRound = null;
                    gameFactionModel.scorePw = 0;
                    //玩家人数
                    gameFactionModel.UserCount = gameInfoModel.UserCount;
                }
                else
                {
                    isAdd = false;
                }
                //修改属性
                gameFactionModel.kjPostion = string.Join("|", faction.TransformLevel, faction.ShipLevel,
                    faction.AILevel, faction.GaiaLevel, faction.EconomicLevel,
                    faction.ScienceLevel);
                gameFactionModel.numberBuild = string.Join("|", 8 - faction.Mines.Count,
                    4 - faction.TradeCenters.Count, 3 - faction.ResearchLabs.Count,
                    faction.Academy1 == null ? 1 : 0, faction.Academy2 == null ? 1 : 0,
                    faction.StrongHold == null ? 1 : 0);
                gameFactionModel.numberFst1 = gaiaGame.FSTList[0].TargetNumber(faction);
                gameFactionModel.numberFst2 = gaiaGame.FSTList[1].TargetNumber(faction);
                gameFactionModel.scoreFst1 = getscore(faction, 0);
                gameFactionModel.scoreFst2 = getscore(faction, 1);
                gameFactionModel.scoreKj = faction.GetTechScoreCount() * 4;
                gameFactionModel.scoreTotal = faction.Score;
                gameFactionModel.rank = rankindex;//排名
                //计算裸分
                gameFactionModel.scoreLuo = gameFactionModel.scoreTotal - gameFactionModel.scoreFst1 -
                                            gameFactionModel.scoreFst2 - gameFactionModel.scoreKj;
                if (isAdd)
                {
                    dbContext.GameFactionModel.Add(gameFactionModel);
                }
                else
                {
                    dbContext.GameFactionModel.Update(gameFactionModel);
                }

                rankindex++;
            }
        }
    }
}
