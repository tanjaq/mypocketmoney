using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.GData.Spreadsheets;
using File = Google.Apis.Drive.v2.Data.File;
using Google.GData.Client;
using Google.GData.Documents;
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
            service = new SpreadsheetsService("MyPocketMoney");
            service.setUserCredentials("jetcarq@gmail.com", "sxgbnaqw1");

        }



        public SpreadsheetEntry GetSetSpreadSheet()
        {

            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = service.Query(query);
            foreach (SpreadsheetEntry entry in feed.Entries)
            {
                if (entry.Title.Text == MyPocketMoney)
                {
                    return entry;

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

                DocumentsService documentsService = new DocumentsService("MyDocumentsListIntegration-v1");
                documentsService.setUserCredentials("jetcarq@gmail.com", "sxgbnaqw1");
                // TODO: Authorize the service object for a specific user (see Authorizing requests)

                // Instantiate a DocumentEntry object to be inserted.
                DocumentEntry documentEntry = new DocumentEntry();

                // Set the document title
                documentEntry.Title.Text = MyPocketMoney;

                // Add the document category
                documentEntry.Categories.Add(DocumentEntry.SPREADSHEET_CATEGORY);

                // Make a request to the API and create the document.
                documentsService.Insert(
                    DocumentsListQuery.documentsBaseUri, documentEntry);

                SpreadsheetFeed spreadsheetFeed = service.Query(query);
                foreach (SpreadsheetEntry entry in spreadsheetFeed.Entries)
                {
                    if (entry.Title.Text == MyPocketMoney)
                    {
                        mainSpreadsheetEntryuri = entry.FeedUri;
                        return entry;

                    }
                }
            }

            return null;

        }
    }
}
