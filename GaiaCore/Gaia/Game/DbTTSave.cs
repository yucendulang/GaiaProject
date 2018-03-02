using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GaiaDbContext.Models.HomeViewModels;

namespace GaiaCore.Gaia.Game
{
    //科技得分
    public class DbTTSave
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="gaiaGame"></param>
        /// <param name="faction"></param>
        /// <param name="isAdd">追加</param>
        public static void Score(Type type,GaiaGame gaiaGame,Faction faction,bool isAdd=true)
        {
            if (type.Name.Contains("ATT") && gaiaGame.dbContext != null && gaiaGame.IsSaveToDb)
            {
                if (type.Name == "ATT11")
                {
                    int a = 1;
                }

                GameFactionExtendModel gameFactionExtendModel = gaiaGame.dbContext.GameFactionExtendModel.SingleOrDefault(
                    item => item.gameinfo_name == gaiaGame.GameName &&
                            item.FactionName == faction.FactionName.ToString());
                if (gameFactionExtendModel != null)
                {

                    //取值
                    string strClass = "GaiaCore.Gaia.Tiles." + type.Name;  //命名空间+类名
                    string strMethod = "GetResources";//方法名

                    Type classtype;
                    object obj;

                    classtype = Type.GetType(strClass);//通过string类型的strClass获得同名类“type”
                    obj = System.Activator.CreateInstance(classtype);//创建type类的实例 "obj"


                    MethodInfo method = type.GetMethod(strMethod, new Type[] { typeof(Faction) });//取的方法描述//2
                    short result = (short)method.Invoke(obj, new object[] { faction, });//3

                    //赋值
                    Type modeltype = gameFactionExtendModel.GetType();
                    //var ps = type.GetProperties();

                    var ps = modeltype.GetProperties().ToList().Find(item => item.Name == type.Name + "Score");
                    if (ps != null)
                    {
                        if (isAdd)
                        {
                            short value = (short)ps.GetValue(gameFactionExtendModel);
                            ps.SetValue(gameFactionExtendModel, (Int16)(result + value), null);
                        }
                        else
                        {
                            ps.SetValue(gameFactionExtendModel, (Int16)(result), null);
                        }
                    }
                    //保存
                    gaiaGame.dbContext.GameFactionExtendModel.Update(gameFactionExtendModel);
                    gaiaGame.dbContext.SaveChanges();

                    //var ti = type.GetTypeInfo();
                    //MethodInfo mtd = ti.GetMethod("GetResources");
                    //var genMethod = mtd.MakeGenericMethod(typeof(int));

                    //var obj = genMethod.Invoke(type, new object[] { });

                    //                        switch (type.ToString())
                    //                        {
                    //                            case "ATT4":
                    //                                gameFactionExtendModel.ATT4Score += (short)new ATT4().GetTriggerScore;
                    //                                break;
                    //
                    //                        }
                    //                        
                }

            }
        }
    }
}
