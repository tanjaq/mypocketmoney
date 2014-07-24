using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v2.Data;
using Google.GData.Client;
using Google.GData.Documents;
using Google.GData.Spreadsheets;
using File = System.IO.File;
using SpreadsheetQuery = Google.GData.Spreadsheets.SpreadsheetQuery;


namespace WpfApplication2.Repos
{
    public class GoogleDocsService
    {
        private const string MyPocketMoney = "MyPocketMoney";
        private SpreadsheetsService service;

        public string mainSpreadsheetEntryuri;
        public GoogleDocsService()
        {
            //service = new SpreadsheetsService("MyPocketMoney");
            //service.setUserCredentials("jetcarq@gmail.com", "sxgbnaqw1");

        }

      

        public SpreadsheetFeed GetSetSpreadSheet()
        {

            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = service.Query(query);
            foreach (SpreadsheetEntry entry in feed.Entries)
            {
                if (entry.Title.Text == MyPocketMoney)
                {
                    mainSpreadsheetEntryuri = entry.FeedUri;
                    return new SpreadsheetFeed(new Uri(mainSpreadsheetEntryuri), service);

                }
                //foreach (var worksheet in entry.Worksheets.Entries)
                //{

                //    AtomLink cellFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel, null);

                //    CellQuery query2 = new CellQuery(cellFeedLink.HRef.ToString());
                //    CellFeed feed2= service.Query(query2);



                //    foreach (CellEntry curCell in feed2.Entries)
                //    {
                //        Console.WriteLine("Row {0}, column {1}: {2}", curCell.Cell.Row,
                //            curCell.Cell.Column, curCell.Cell.Value);
                //    }
                //}
            }
            if (mainSpreadsheetEntryuri == null)
            {

                DocumentsService insertservice = new Google.GData.Documents.DocumentsService(MyPocketMoney);
                service.setUserCredentials("jetcarq@gmail.com", "sxgbnaqw1");

                Google.GData.Documents.SpreadsheetQuery insertquery = new Google.GData.Documents.SpreadsheetQuery();
                insertquery.Title = MyPocketMoney;
                insertquery.TitleExact = true;

                Google.GData.Documents.DocumentsFeed insertfeed = insertservice.Query(insertquery);
                Google.GData.Client.AtomEntry entry = insertfeed.Entries[0];

                var feedUri = new Uri(Google.GData.Documents.DocumentsListQuery.documentsBaseUri);

                service.Insert(feedUri, entry);

                //var timeranges = new WorksheetEntry();
                //timeranges.Title.Text = "TimeRanges";
                //service.Insert(newSpreadsheedEntry.Feed, timeranges);

                //var records = new WorksheetEntry();
                //records.Title.Text = "Records";
                //service.Insert(newSpreadsheedEntry.Feed, records);
                //mainSpreadsheetEntryuri = newSpreadsheedEntry.FeedUri;

            }

            return new SpreadsheetFeed(new Uri(mainSpreadsheetEntryuri), service);

        }

        public void CreateSpreadSheet()
        {

            DocumentsService insertservice = new Google.GData.Documents.DocumentsService(MyPocketMoney);
            insertservice.setUserCredentials("jetcarq@gmail.com", "sxgbnaqw1");

            Google.GData.Documents.SpreadsheetQuery insertquery = new Google.GData.Documents.SpreadsheetQuery();
            insertquery.Title = MyPocketMoney;
            insertquery.TitleExact = true;

            Google.GData.Documents.DocumentsFeed insertfeed = insertservice.Query(insertquery);
            Google.GData.Client.AtomEntry entry = insertfeed.Entries[0];

            var feedUri = new Uri(Google.GData.Documents.DocumentsListQuery.documentsBaseUri);

            service.Insert(feedUri, entry);
        }
    }
}
