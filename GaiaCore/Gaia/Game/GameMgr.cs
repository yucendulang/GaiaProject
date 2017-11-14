﻿using Newtonsoft.Json;
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
        public static bool CreateNewGame(string name, string[] username, out GaiaGame result, int seed = 0, bool isTestGame = false)
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
                result.Syntax(GameSyntax.setupGame+ seed,out string log);
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

        public static IEnumerable<string> GetAllGame(string userName=null)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return m_dic.Keys;
            }
            else
            {
                var result=from p in m_dic where p.Value.Username.Contains(userName) select p.Key;
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
                var result = GetAllGame(userName).ToList().Find(x => {
                    var gg = GetGameByName(x);
                    return gg.UserDic.Count > 1 && gg.GetCurrentUserName().Equals(userName)&&gg.GameStatus.stage!=Stage.GAMEEND;
                });
                return result;
            }
        }

        public static bool BackupDictionary()
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.ContractResolver = new LimitPropsContractResolver(new string[] { "UserActionLog", "Username" , "IsTestGame" });
            var str=JsonConvert.SerializeObject(m_dic,Formatting.Indented, jsetting);
            var logPath = System.IO.Path.Combine(BackupDataPath, DateTime.Now.ToString("yyyyMMddHHmmss")+".txt");
            var logWriter=System.IO.File.CreateText(logPath);
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

        public static async System.Threading.Tasks.Task<IEnumerable<string>> RestoreDictionaryFromServerAsync()
        {
            HttpClient client = new HttpClient();
            var logReader = await client.GetStringAsync("http://gaiaproject.chinacloudsites.cn/home/GetLastestActionLog");
            return RestoreAllGames(logReader);
        }

        private static IEnumerable<string> RestoreAllGames(string logReader)
        {
            var temp = JsonConvert.DeserializeObject<Dictionary<string, GaiaGame>>(logReader);
            m_dic = new Dictionary<string, GaiaGame>();
            foreach (var item in temp)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("开始恢复" + item.Key);
                    var gg = new GaiaGame(item.Value.Username);
                    gg.IsTestGame = item.Value.IsTestGame;
                    foreach (var str in item.Value.UserActionLog.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        gg.Syntax(str, out string log);
                        System.Diagnostics.Debug.WriteLine(log);
                    }
                    m_dic.Add(item.Key, gg);
                }
                catch
                {
                    continue;
                }
            }
            return m_dic.Keys;
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
            System.Diagnostics.Debug.WriteLine("读取文件" + filename);
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
    }
}
