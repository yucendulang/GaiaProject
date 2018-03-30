using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gaia.Service;
using GaiaDbContext.Models;
using GaiaDbContext.Models.HomeViewModels;
using GaiaProject.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
            //开始保存数据
            if (!gaiaGame.IsSaveToDb)
            {
                return;
            }
            //            //如果游戏没有结束不保存数据
            //            if (gaiaGame.GameStatus.status != Status.ENDED)
            //            {
            //                return;
            //            }
            if (gaiaGame.dbContext == null)
            {
                return;
            }
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
                    //获取排名
                    int rank = gaiaGame.dbContext.GameFactionModel
                        .SingleOrDefault(item => item.gameinfo_name == gaiaGame.GameName &&
                                                 item.username == faction.UserName)?.rank??0;

                    extendModel = new GameFactionExtendModel()
                    {
                        gameinfo_name = gaiaGame.GameName, //名称
                        FactionName = faction.FactionName.ToString(), //种族   
                        username = faction.UserName,
                        //rank = faction.,
                        rank = rank,
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

            //var server = gaiaGame.dbContext.Database.GetService<this>();
        }



        public static IEmailSender _emailSender;

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
                //获取游戏
                var gameinfo = dbContext.GameInfoModel.SingleOrDefault(item => item.name == gaiaGame.GameName);
                if (gameinfo == null)
                {
                    return;
                }
                string finalScore = string.Join(":",
                    gaiaGame.FactionList.OrderByDescending(item => item.Score)
                        .Select(item => string.Format("{0}{1}({2})", item.ChineseName,
                            item.Score, item.UserName)));

                //已经结束的不发送邮件
                if (gameinfo.GameStatus == 8)
                {
                   return;
                }
                else
                {
                     string title = $"游戏({gaiaGame.GameName})结束,{finalScore}";
                    string message = "http://totoman.online/Home/RestoreGame/" + gameinfo.Id.ToString();
                    //向全部玩家发送游戏结束提醒
                    try
                    {
                        foreach (string s in gaiaGame.Username)
                        {
                            ApplicationUser applicationUser = dbContext.Users.SingleOrDefault(user => user.UserName == s);
                            if (applicationUser != null)
                            {
                                _emailSender?.SendEmailAsync(applicationUser.Email, title, message);
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }

                //保存
                if (ApplicationDbContext.isSaveResult)
                {
                    try
                    {
                        //保存游戏结束时的信息
                        //先保存游戏信息
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
                        gameinfo.scoreFaction = finalScore; //最后的得分情况

                        dbContext.GameInfoModel.Update(gameinfo);


                        //dbContext.SaveChanges();
                        //保存种族信息
                        try
                        {
                            SaveFactionToDb(dbContext, gaiaGame, gameinfo);
                        }
                        catch (Exception e)
                        {

                        }
                        dbContext.SaveChanges();

                        //重新执行程序，计算数据
                        //GameMgr.RestoreGame(gaiaGame.GameName, gaiaGame, isToDict:false,isSaveToDb:true);
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
            //最高分
            int scoreMax = 0;
            List<GameFactionModel> gameFactionModels = new List<GameFactionModel>(factionList.Count);
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
                //记录最高分
                if (rankindex == 1)
                {
                    scoreMax = gameFactionModel.scoreTotal;
                }
                //如果是后面的玩家并且与上一名玩家分数相同
                if (rankindex > 1 && gameFactionModel.scoreTotal == factionList[rankindex - 2].Score)
                {
                    gameFactionModel.rank = gameFactionModels[rankindex - 2].rank;
                    //gameFactionModel.rank = dbContext.GameFactionModel.SingleOrDefault(item => item.gameinfo_id == gameInfoModel.Id && item.FactionName == factionList[rankindex - 2].FactionName.ToString()).rank;//排名
                }
                else
                {
                    gameFactionModel.rank = rankindex;//排名
                    //faction.
                }
                //计算裸分
                gameFactionModel.scoreLuo = gameFactionModel.scoreTotal - gameFactionModel.scoreFst1 -
                                            gameFactionModel.scoreFst2 - gameFactionModel.scoreKj;
                //计算分差
                gameFactionModel.scoreDifference = scoreMax - gameFactionModel.scoreTotal;
                //添加到数组
                gameFactionModels.Add(gameFactionModel);
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

            int a = 1;
        }
    }
}
