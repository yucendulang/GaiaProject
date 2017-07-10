using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia
{
    public static class GameMgr
    {
        private static Dictionary<string, GaiaGame> m_dic;
        static GameMgr()
        {
            m_dic = new Dictionary<string, GaiaGame>();
        }
        public static bool CreateNewGame(string name,out GaiaGame result)
        {
            if (m_dic.ContainsKey(name))
            {
                result = null;
                return false;
            }
            else
            {
                result = new GaiaGame();
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

        public static bool BakeDictionary()
        {
            var str=JsonConvert.SerializeObject(m_dic);
            var logPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "123.txt");
            var logWriter=System.IO.File.CreateText(logPath);
            logWriter.Write(str);
            logWriter.Dispose();
            return true;
        }
    }
}
