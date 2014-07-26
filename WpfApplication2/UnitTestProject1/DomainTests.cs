using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApplication2.DomainObjects;

namespace UnitTestProject1
{
    [TestClass]
    public class DomainTests
    {
        [TestMethod]
        public void TestRecord()
        {
            Record rc = new Record("test",12,true);
            var data = rc.GetDataForSaving();
            Assert.AreEqual("test",data["Name"]);

        }
        [TestMethod]
        public void TestTimerange()
        {
            var rc = new TimeRange(12){Name = "test1"};
            var data = rc.GetDataForSaving();
            Assert.AreEqual("test1",data["Name"]);

        }
    }
}
