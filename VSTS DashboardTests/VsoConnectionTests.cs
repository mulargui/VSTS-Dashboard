using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSTS_Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSTS_Dashboard.Tests
{
    [TestClass()]
    public class VsoConnectionTests
    {
        [TestMethod()]
        public void GetStatusTest()
        {
            const string workitemid = "41018";
            //string responseBody;

            VsoConnection d = new VsoConnection();
            var task = new Task(() => d.GetStatus(workitemid));
            task.RunSynchronously();

            Assert.IsTrue(task.IsCompleted, "GetStatusTest() passed");
        }
    }
}