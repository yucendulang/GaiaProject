using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GaiaCore.Gaia.User
{
    class UserMgr
    {
    }
    public static class PowerUser
    {
        public static List<string> PowerUserList = new List<string>()
        {
            "yucenyucen@126.com",
            "xsssssssch@hotmail.com",
            "toto",
            "totofans",
        };
        public static bool IsPowerUser(string username)
        {
            
            var ret=PowerUserList.Contains(username);
            return ret;
        }

    }
}
