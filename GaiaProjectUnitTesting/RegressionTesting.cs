using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GaiaCore.Gaia;

namespace GaiaProjectUnitTesting
{
    [TestClass]
    public class RegressionTesting
    {
        [TestMethod]
        public void RegressionTestingFromServer()
        {
            Func<string, bool> func = (string a) =>
              {
                  System.Diagnostics.Debug.WriteLine(a);
                  Assert.IsNull(a);
                  return true;
              };
            var task=GameMgr.RestoreDictionaryFromServerAsync(DebugInvoke: func);
            task.Wait();
        }
    }
}
