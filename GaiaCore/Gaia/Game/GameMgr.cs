using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using GaiaCore.Util;
using System.Net.Http;
using GaiaDbContext.Models.AccountViewModels;

namespace GaiaCore.Gaia
{

    public static class GameMgr
    {
        private static Dictionary<string, GaiaGame> m_dic;
        static GameMgr()
        {
            m_dic = new Dictionary<string, GaiaGame>();
        }

        public static bool CreateNewGame(string name, string[] username, out GaiaGame result, string MapSelection, int seed = 0, bool isTestGame = false,bool isSocket = false)
        {
            if (m_dic.ContainsKey(name))
            {
                result = null;
                return false;
            }
            else
            {
                seed = seed == 0 ? RandomInstance.Next(int.MaxValue) : seed;
                result = new GaiaGame(username,name);
                result.IsTestGame = isTestGame;
                result.Syntax(GameSyntax.setupmap + " " + MapSelection, out string log);
                result.Syntax(GameSyntax.setupGame + seed, out log);
                result.GameName = name;//游戏名称
                result.IsSocket = isSocket;//即时制
                m_dic.Add(name, result);
                return true;
            }
        }

        public static void RemoveOldBackupData()
        {
            var d = new DirectoryInfo(BackupDataPath);
            var filename = (from p in d.EnumerateFiles() orderby p.Name descending select p.Name).Take(5);
            foreach (var item in d.EnumerateFiles())
            {
                if (!filename.Contains(item.Name))
                {
                    System.IO.File.Delete(System.IO.Path.Combine(BackupDataPath, item.Name));
                }
            }
        }

        public static GaiaGame GetGameByName(string name)
        {
            if (name == null)
            {
                return null;
            }
            if (m_dic.ContainsKey(name))
            {
                GaiaGame gaiaGame = m_dic[name];
                if (gaiaGame.GameName == null)
                {
                    gaiaGame.GameName = name;
                }
                return gaiaGame;
            }
            else
            {
                return null;
            }
        }

        public static bool RemoveAndBackupGame(string name)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.ContractResolver = new LimitPropsContractResolver(new string[] { "UserActionLog", "Username", "IsTestGame" });
            var str = JsonConvert.SerializeObject(m_dic[name], Formatting.Indented, jsetting);
            var logPath = System.IO.Path.Combine(FinishGamePath, name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
            var logWriter = System.IO.File.CreateText(logPath);
            logWriter.Write(str);
            logWriter.Dispose();
            m_dic.Remove(name);
            return true;
        }

        public static IEnumerable<string> GetAllGameName(string userName = null)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return m_dic.Keys;
            }
            else
            {
                var result = from p in m_dic where p.Value.Username.Contains(userName) select p.Key;
                return result;
            }
        }

        public static IEnumerable<KeyValuePair<string, GaiaGame>> GetAllGame(string userName = null)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return m_dic;
            }
            else
            {
                var result = from p in m_dic where p.Value.Username.Contains(userName) select p;
                return result;
            }
        }

        public static string GetNextGame(string userName = null)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return string.Empty;
            }
            else
            {
                var result = GetAllGameName(userName).ToList().Find(x =>
                {
                    var gg = GetGameByName(x);
                    if (!gg.UserGameModels.Find(item => item.username == userName).isTishi)
                    {
                        return false;
                    }
                    else
                    {
                        var isLeech = gg.UserDic.Count > 1 && gg.GameStatus.stage == Stage.ROUNDWAITLEECHPOWER && gg.UserDic.ContainsKey(userName) && gg.UserDic[userName].Exists(y => y.LeechPowerQueue.Count != 0);
                        bool flag = isLeech || (gg.UserDic.Count > 1 && gg.GetCurrentUserName().Equals(userName) && gg.GameStatus.stage != Stage.GAMEEND);
                        return flag;
                    }
                });
                return result;
            }
        }

        public static bool BackupDictionary()
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.ContractResolver = new LimitPropsContractResolver(new string[]
            {
                "GameName",  "UserActionLog", "Username", "IsTestGame", "LastMoveTime", "version",
                "UserGameModels","username","remark","isTishi","IsSocket"
            });
            var str = JsonConvert.SerializeObject(m_dic, Formatting.Indented, jsetting);
            var logPath = System.IO.Path.Combine(BackupDataPath, DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
            var logWriter = System.IO.File.CreateText(logPath);
            logWriter.Write(str);
            logWriter.Dispose();
            return true;
        }

        public static IEnumerable<string> RestoreDictionary(string filename)
        {
            string logReader = GetLastestBackupData(filename);
            if (string.IsNullOrEmpty(logReader))
            {
                return null;
            }
            return RestoreAllGames(logReader);
        }

        public static async System.Threading.Tasks.Task<IEnumerable<string>> RestoreDictionaryFromServerAsync(string GameName = null, Func<string, bool> DebugInvoke = null)
        {
            HttpClient client = new HttpClient();
            var logReader = await client.GetStringAsync("http://gaiaproject.chinacloudsites.cn/home/GetLastestActionLog");
            return RestoreAllGames(logReader, GameName, "yucenyucen@126.com", DebugInvoke);
            //return RestoreAllGames(logReader, GameName, DebugInvoke: DebugInvoke);
        }

        private static IEnumerable<string> RestoreAllGames(string logReader, string GameName = null, string user = null, Func<string, bool> DebugInvoke = null)
        {
            var temp = JsonConvert.DeserializeObject<Dictionary<string, GaiaGame>>(logReader);
            m_dic = new Dictionary<string, GaiaGame>();
            foreach (var item in temp)
            {
                if (!string.IsNullOrEmpty(GameName) && !item.Key.Equals(GameName))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(user))
                {
                    for (int i = 0; i < item.Value.Username.Where(x => !string.IsNullOrEmpty(x)).Count(); i++)
                    {
                        item.Value.Username[i] = user;
                    }
                }
                RestoreGameWithActionLog(item, DebugInvoke);

            }
            return m_dic.Keys;
        }

        private static GaiaGame RestoreGameWithActionLog(KeyValuePair<string, GaiaGame> item, Func<string, bool> DebugInvoke = null,bool isTodict=true,int? row=null)
        {
            var gg = new GaiaGame(item.Value.Username,item.Value.GameName);
            gg.IsTestGame = item.Value.IsTestGame;//测试
            gg.IsSocket = item.Value.IsSocket;//即使制度
            if (item.Value.version == 0)
            {
                gg.version = 1;
            }
            else
            {
                gg.version = item.Value.version;
            }
            
            
            try
            {
                int rowIndex = 1;
                foreach (var str in item.Value.UserActionLog.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {

                    gg.Syntax(str, out string log);
                    if (!string.IsNullOrEmpty(log))
                    {
                        if (DebugInvoke != null)
                        {
                            DebugInvoke.Invoke(item.Key + ":" + log);
                        }
                        System.Diagnostics.Debug.WriteLine(item.Key + ":" + log);
                        break;
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine(str);
                    }
                    if (row != null)
                    {
                        //相等，终止恢复
                        if (rowIndex == row)
                        {
                            break;
                        }
                        else
                        {
                            rowIndex++;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                if (DebugInvoke != null)
                {
                    DebugInvoke.Invoke(item.Key + ":" + ex.ToString());
                }
                System.Diagnostics.Debug.WriteLine(item.Key + ":" + ex.ToString());
            }
            gg.LastMoveTime = item.Value.LastMoveTime;
            gg.UserGameModels = item.Value.UserGameModels;
            //需要加载到内存
            if (isTodict)
            {
                if (m_dic.ContainsKey(item.Key))
                {
                    m_dic[item.Key] = gg;
                }
                else
                {
                    m_dic.Add(item.Key, gg);
                }
            }
            else
            {
//                if (m_dic.ContainsKey(item.Key))
//                {
//                    m_dic.Remove(item.Key);
//                }
            }
            return gg;
        }

        public static bool ReportBug(string id)
        {
            var gg = GetGameByName(id);
            if (gg == null)
            {
                return false;
            }
            string[] newUserName = new string[4];
            for (int i = 0; i < 4; i++)
            {
                if (string.IsNullOrEmpty(gg.Username[i]))
                {
                    newUserName[i] = null;
                }
                else
                {
                    newUserName[i] = "Report@Bug.com";
                }
            }
            var ggNew = new GaiaGame(newUserName);
            ggNew.UserActionLog = gg.UserActionLog;
            RestoreGameWithActionLog(new KeyValuePair<string, GaiaGame>(id + "Debug" + DateTime.Now.ToString("yyyyMMddHHmmss"), ggNew));
            return true;
        }

        public static void ChangeAllGamesUsername(string userName1, string userName2)
        {
            if (string.IsNullOrEmpty(userName1) || string.IsNullOrEmpty(userName2))
            {
                return;
            }
            foreach (var item in m_dic)
            {
                for (int i = 0; i < item.Value.Username.Length; i++)
                {
                    if (item.Value.Username[i] != null && item.Value.Username[i].Equals(userName1))
                    {
                        item.Value.Username[i] = userName2;
                    }
                }
                var needModify = item.Value.UserDic.Where(x => x.Key.Equals(userName1));
                needModify.ToList().ForEach(x =>
                {
                    item.Value.UserDic.Add(userName2, x.Value);
                    item.Value.UserDic.Remove(x.Key);
                });
                foreach(var fac in item.Value.FactionList)
                {
                    if (fac.UserName.Equals(userName1))
                    {
                        fac.UserName = userName2;
                    }
                }
            }
        }

        public static bool RedoOneStep(string id)
        {
            var gg = GetGameByName(id);
            if (gg == null|| !gg.RedoStack.Any())
            {
                return false;
            }
            var syntax = gg.RedoStack.Pop();
            gg.Syntax(syntax, out string log);
            return true;
        }

        public static bool DeleteOneGame(string id)
        {
            if (m_dic.ContainsKey(id))
            {
                m_dic.Remove(id);
            }
            return true;
        }

        public static void DeleteAllGame()
        {
            m_dic.Clear();
        }

        /// <summary>
        /// 返回读取到的文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetLastestBackupData(string filename = null)
        {
            if (string.IsNullOrEmpty(filename))
            {
                var d = new DirectoryInfo(BackupDataPath);
                filename = (from p in d.EnumerateFiles() orderby p.Name descending select p.Name).FirstOrDefault();
            }
            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }
            var logPath = Path.Combine(BackupDataPath, filename);
            var logReader = File.ReadAllText(logPath);
            return logReader;
        }

        public static IEnumerable<string> GetAllBackupDataName()
        {
            var d = new DirectoryInfo(BackupDataPath);
            return from p in d.EnumerateFiles() select p.Name;
        }

        private static string BackupDataPath
        {
            get
            {
                if (!Directory.Exists("backupdata"))
                {
                    Directory.CreateDirectory("backupdata");
                }
                return System.IO.Path.Combine(Directory.GetCurrentDirectory(), "backupdata");
            }
        }

        private static string FinishGamePath
        {
            get
            {
                if (!Directory.Exists("finishgame"))
                {
                    Directory.CreateDirectory("finishgame");
                }
                return System.IO.Path.Combine(Directory.GetCurrentDirectory(), "finishgame");
            }
        }
        /// <summary>
        /// 后退一步
        /// </summary>
        /// <param name="GameName"></param>
        /// <returns></returns>
        public static bool UndoOneStep(string GameName)
        {
            var gg = GetGameByName(GameName);
            if (gg == null)
            {
                return false;
            }
            var syntaxList = gg.UserActionLog.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (syntaxList.Last().StartsWith("#"))
            {
                while (syntaxList.Last().StartsWith("#"))
                {
                    syntaxList.RemoveAt(syntaxList.Count - 1);
                }
            }
            gg.RedoStack.Push(syntaxList.Last());
            var Redo = gg.RedoStack;
            syntaxList.RemoveAt(syntaxList.Count - 1);


            gg.UserActionLog = string.Join("\r\n", syntaxList);

            RestoreGameWithActionLog(new KeyValuePair<string, GaiaGame>(GameName, gg));
            GetGameByName(GameName).RedoStack = Redo;
            return true;
        }

        public static GaiaGame RestoreGame(string GameName,GaiaGame gg,int? row=null)
        {
            return RestoreGameWithActionLog(new KeyValuePair<string, GaiaGame>(GameName, gg),null,false,row:row);
        }
    }
}
