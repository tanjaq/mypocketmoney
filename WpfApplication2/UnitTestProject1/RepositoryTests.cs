using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Google.GData.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApplication2.DomainObjects;
using WpfApplication2.Repos;

namespace UnitTestProject1
{
    [TestClass]
    public class RepositoryTests
    {


        //[TestMethod]
        //public void TestUploadSheet()
        //{
        //    DocumentsService service = new DocumentsService("MyDocumentsListIntegration-v1");
        //    service.setUserCredentials("jetcarq@gmail.com", "sxgbnaqw1");

        //    // Instantiate a DocumentEntry object to be inserted.
        //    DocumentEntry entry = new DocumentEntry();

        //    // Set the document title
        //    entry.Title.Text = "Legal Contract2";

        //    // Add the document category
        //    entry.Categories.Add(DocumentEntry.SPREADSHEET_CATEGORY);

        //    // Make a request to the API and create the document.
        //    DocumentEntry newEntry = service.Insert(
        //        DocumentsListQuery.documentsBaseUri, entry);
        //}

        [TestMethod]
        public void CreateSheet()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
            GoogleDocsService service = new GoogleDocsService();
            var sheet = service.GetSetSpreadSheet();
            Assert.IsNotNull(sheet);
        }

        [TestMethod]
        public void InsertRecordLoadBack()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
            GoogleDocsService service = new GoogleDocsService();
            var sheet = service.GetSetSpreadSheet();
            Assert.IsNotNull(sheet);

            var record = service.InsertUpdate(new Record("test", 10, true));
            Assert.IsTrue(record.Id > 0);
            var loadedRecord = service.GetById<Record>(record.Id);
            Assert.AreEqual("test", loadedRecord.Name);
            Assert.AreEqual(10M, loadedRecord.Amount);
            service.Delete(loadedRecord);

            var loadedDeletedRecord = service.GetById<Record>(record.Id);
            Assert.IsNull(loadedDeletedRecord);

        }
        [TestMethod]
        public void InsertRecordUpdateLoadBack()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
            GoogleDocsService service = new GoogleDocsService();
            var sheet = service.GetSetSpreadSheet();
            Assert.IsNotNull(sheet);

            var record = service.InsertUpdate(new Record("test", 10, true) { PaidTime = DateTime.Now });
            Assert.IsTrue(record.Id > 0);
            var id = record.Id;
            var loadedRecord = service.GetById<Record>(record.Id);
            Assert.AreEqual("test", loadedRecord.Name);
            Assert.AreEqual(10M, loadedRecord.Amount);

            loadedRecord.Name = "ChangedTest";
            service.InsertUpdate(loadedRecord);
            var loadedDeletedRecord = service.GetById<Record>(record.Id);
            Assert.IsNotNull(loadedDeletedRecord);
            Assert.AreEqual("ChangedTest", loadedDeletedRecord.Name);
            Assert.AreEqual(id, loadedDeletedRecord.Id);
            service.Delete(loadedDeletedRecord);
            loadedRecord = service.GetById<Record>(record.Id);
            Assert.IsNull(loadedRecord);



        }


        [TestMethod]
        public void InsertTimeRangeLoadBackDelete()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
            GoogleDocsService service = new GoogleDocsService();
            var sheet = service.GetSetSpreadSheet();
            Assert.IsNotNull(sheet);

            var timeRange = service.InsertUpdate(new TimeRange() { InitialAmount = 3000, StartDate = DateTime.Today, Name = "avgust" });
            Assert.IsTrue(timeRange.Id > 0);
            var loadedRecord = service.GetById<TimeRange>(timeRange.Id);
            Assert.AreEqual("avgust", loadedRecord.Name);
            Assert.AreEqual(3000M, loadedRecord.InitialAmount);
            Assert.AreEqual(DateTime.Today, loadedRecord.StartDate);

            service.Delete(loadedRecord);

            var loadedDeletedRecord = service.GetById<Record>(timeRange.Id);
            Assert.IsNull(loadedDeletedRecord);

        }

        [TestMethod]
        public void GetByQuery()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
            GoogleDocsService service = new GoogleDocsService();
            var sheet = service.GetSetSpreadSheet();
            Assert.IsNotNull(sheet);

            var timeRange = service.InsertUpdate(new TimeRange() { InitialAmount = 3000, StartDate = DateTime.Today, Name = "test" });
            service.InsertUpdate(new TimeRange() { InitialAmount = 3000, StartDate = DateTime.Today, Name = "test" });
            Assert.IsTrue(timeRange.Id > 0);
            var timeRanges = service.GetByQuery<TimeRange>("name = {0}", "test");
            Assert.IsTrue(timeRanges.Count > 1);
            Assert.AreEqual("test", timeRanges[0].Name);
            Assert.AreEqual(3000M, timeRanges[0].InitialAmount);
            Assert.AreEqual(DateTime.Today, timeRanges[0].StartDate);
            foreach (var range in timeRanges)
            {
                service.Delete(range);

            }

            var loadedDeletedRecord = service.GetByQuery<TimeRange>("name = {0}", "test");
            Assert.AreEqual(0, loadedDeletedRecord.Count);

        }


        [TestMethod]
        public void LoadAll()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
            GoogleDocsService service = new GoogleDocsService();
            var sheet = service.GetSetSpreadSheet();
            Assert.IsNotNull(sheet);

            var timeRange = service.InsertUpdate(new TimeRange() { InitialAmount = 3000, StartDate = DateTime.Today, Name = "test" });
            service.InsertUpdate(new TimeRange() { InitialAmount = 3000, StartDate = DateTime.Today, Name = "test" });
            Assert.IsTrue(timeRange.Id > 0);
            var timeRanges = service.GetByQuery<TimeRange>("");
            Assert.IsTrue(timeRanges.Count > 1);
            Assert.AreEqual("test", timeRanges[0].Name);
            Assert.AreEqual(3000M, timeRanges[0].InitialAmount);
            Assert.AreEqual(DateTime.Today, timeRanges[0].StartDate);
            foreach (var range in timeRanges)
            {
                service.Delete(range);

            }

            var loadedDeletedRecord = service.GetByQuery<TimeRange>("");
            Assert.AreEqual(0, loadedDeletedRecord.Count);

        }



        //[TestMethod]
        //public void SaveLoadJsonStorage()
        //{
        //    LocalXmlStorage service = new LocalXmlStorage();
        //    RepoCollection repo = new RepoCollection();
        //    repo.Records = new List<Record>()
        //    {
        //        new Record("22", 1, true) {Visibility = Visibility.Collapsed},
        //        new Record("22", 1, true) {Visibility = Visibility.Hidden},
        //        new Record("22", 1, true) {Visibility = Visibility.Visible},
        //    };

        //    repo.TimeRanges = new List<TimeRange>()
        //    {
        //        new TimeRange(1),
        //        new TimeRange(2),
        //    };

        //    var json = service.SerializeAndSaveFile(repo);

        //    Assert.IsTrue(!json.ToLower().Contains("childs"));

        //    var records = service.DeserializeFile();

        //    Assert.AreEqual(3, records.Records.Count);
        //    Assert.AreEqual(2, records.TimeRanges.Count);
        //    Assert.AreEqual(Visibility.Collapsed, records.Records[0].Visibility);
        //    Assert.AreEqual(Visibility.Hidden, records.Records[1].Visibility);
        //    Assert.AreEqual(Visibility.Visible, records.Records[2].Visibility);
        //}


    }
}
