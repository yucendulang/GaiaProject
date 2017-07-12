using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using GaiaCore.Util;

namespace GaiaCore.Gaia
{

    public static class GameMgr
    {
        private static Dictionary<string, GaiaGame> m_dic;
        static GameMgr()
        {
            m_dic = new Dictionary<string, GaiaGame>();
        }
        public static bool CreateNewGame(string name, string[] username,out GaiaGame result,int seed=0)
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
                result.ProcessSyntax(GameSyntax.setupGame+ seed, out string log);
                m_dic.Add(name, result);
                return true;
            }           
        }

        public static GaiaGame GetGameByName(string name)
        {
            if (m_dic.ContainsKey(name))
            {
                return m_dic[name];
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<string> GetAllGame()
        {
            return m_dic.Keys;
        }

        public static bool BackupDictionary()
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.ContractResolver = new LimitPropsContractResolver(new string[] { "UserActionLog", "Username" });
            var str=JsonConvert.SerializeObject(m_dic,Formatting.Indented, jsetting);
            var logPath = System.IO.Path.Combine(BackupDataPath, DateTime.Now.ToString("yyyyMMddhhmmss")+".txt");
            var logWriter=System.IO.File.CreateText(logPath);
            logWriter.Write(str);
            logWriter.Dispose();
            return true;
        }

        public static IEnumerable<string> RestoreDictionary(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                var d = new DirectoryInfo(BackupDataPath);
                filename = (from p in d.EnumerateFiles() orderby p.Name descending select p.Name).FirstOrDefault() ;
            }
            var logPath = Path.Combine(BackupDataPath, filename);
            var logReader = File.ReadAllText(logPath);
            var temp = JsonConvert.DeserializeObject<Dictionary<string,GaiaGame>>(logReader);
            m_dic = new Dictionary<string, GaiaGame>();
            foreach (var item in temp)
            {
                var gg = new GaiaGame(item.Value.Username);
                foreach(var str in item.Value.UserActionLog.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    gg.ProcessSyntax(str,out string log);
                }
                m_dic.Add(item.Key, gg);
            }
            return m_dic.Keys;
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
    }
}
