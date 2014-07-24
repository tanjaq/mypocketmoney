using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApplication2.DomainObjects;
using WpfApplication2.Repos;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetAllSheets()
        {
            GoogleDocsService service = new GoogleDocsService();
            //var entry = service.GetSetSpreadSheet();
            //Assert.IsNotNull(entry);
        }

        [TestMethod]
        public void CreateSheet()
        {
            //GoogleDocsService service = new GoogleDocsService();
            //service.CreateSpreadSheet();
        }


        [TestMethod]
        public void SaveLoadJsonStorage()
        {
            LocalXmlStorage service = new LocalXmlStorage();
            RepoCollection repo = new RepoCollection();
            repo.Records = new List<Record>()
            {
                new Record("22", 1, true) {Visibility = Visibility.Collapsed},
                new Record("22", 1, true) {Visibility = Visibility.Hidden},
                new Record("22", 1, true) {Visibility = Visibility.Visible},
            };

            repo.TimeRanges = new List<TimeRange>()
            {
                new TimeRange(1),
                new TimeRange(2),
            };

            var json = service.SerializeAndSaveFile(repo);

            Assert.IsTrue(!json.ToLower().Contains("childs"));

            var records = service.DeserializeFile();

            Assert.AreEqual(3, records.Records.Count);
            Assert.AreEqual(2, records.TimeRanges.Count);
            Assert.AreEqual(Visibility.Collapsed, records.Records[0].Visibility);
            Assert.AreEqual(Visibility.Hidden, records.Records[1].Visibility);
            Assert.AreEqual(Visibility.Visible, records.Records[2].Visibility);
        }


    }
}
