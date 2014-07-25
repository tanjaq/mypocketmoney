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
    public class UnitTest1
    {
     
        
        [TestMethod]
        public void TestUploadSheet()
        {
            DocumentsService service = new DocumentsService("MyDocumentsListIntegration-v1");
            service.setUserCredentials("jetcarq@gmail.com","sxgbnaqw1");
            // TODO: Authorize the service object for a specific user (see Authorizing requests)

            // Instantiate a DocumentEntry object to be inserted.
            DocumentEntry entry = new DocumentEntry();

            // Set the document title
            entry.Title.Text = "Legal Contract2";

            // Add the document category
            entry.Categories.Add(DocumentEntry.SPREADSHEET_CATEGORY);

            // Make a request to the API and create the document.
            DocumentEntry newEntry = service.Insert(
                DocumentsListQuery.documentsBaseUri, entry);
        }

        [TestMethod]
        public void CreateSheet()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
            GoogleDocsService service = new GoogleDocsService();
            var sheet = service.GetSetSpreadSheet();
            Assert.IsNotNull(sheet);
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
