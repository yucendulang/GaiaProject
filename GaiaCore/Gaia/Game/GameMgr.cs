using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using GaiaCore.Util;
using System.Net.Http;

namespace GaiaCore.Gaia
{

    public static class GameMgr
    {
        private static Dictionary<string, GaiaGame> m_dic;
        static GameMgr()
        {
            m_dic = new Dictionary<string, GaiaGame>();
        }
        public static bool CreateNewGame(string name, string[] username, out GaiaGame result, string MapSelection, int seed = 0, bool isTestGame = false)
        {
            if (m_dic.ContainsKey(name))
            {
                result = null;
                return false;
            }
            else
            {
                seed = seed == 0 ? RandomInstance.Next(int.MaxValue) : seed;
                result = new GaiaGame(username);
                result.IsTestGame = isTestGame;
                result.Syntax(GameSyntax.setupmap + " " + MapSelection, out string log);
                result.Syntax(GameSyntax.setupGame + seed, out log);
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
                return m_dic[name];
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
                    var isLeech = gg.UserDic.Count > 1 && gg.GameStatus.stage == Stage.ROUNDWAITLEECHPOWER && gg.UserDic.ContainsKey(userName) && gg.UserDic[userName].Exists(y => y.LeechPowerQueue.Count != 0);
                    return isLeech || (gg.UserDic.Count > 1 && gg.GetCurrentUserName().Equals(userName) && gg.GameStatus.stage != Stage.GAMEEND);
                });
                return result;
            }
        }

        public static bool BackupDictionary()
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.ContractResolver = new LimitPropsContractResolver(new string[] { "UserActionLog", "Username", "IsTestGame", "LastMoveTime" });
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

        private static void RestoreGameWithActionLog(KeyValuePair<string, GaiaGame> item, Func<string, bool> DebugInvoke = null)
        {
            var gg = new GaiaGame(item.Value.Username);
            gg.IsTestGame = item.Value.IsTestGame;
            
            try
            {
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
            if (m_dic.ContainsKey(item.Key))
            {
                m_dic[item.Key] = gg;
            }
            else
            {
                m_dic.Add(item.Key, gg);
            }
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
    }
}
