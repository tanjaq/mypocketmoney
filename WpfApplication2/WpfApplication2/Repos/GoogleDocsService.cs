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

        private SpreadsheetEntry mainSpreadSheet;
        private WorksheetEntry Timeranges;
        private WorksheetEntry Records;

        public GoogleDocsService()
        {
            service = new SpreadsheetsService(MyPocketMoney);
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
                    mainSpreadSheet = entry;
                    return entry;

                }
            }
            if (mainSpreadSheet == null)
            {

                DocumentsService documentsService = new DocumentsService(MyPocketMoney);
                documentsService.setUserCredentials("jetcarq@gmail.com", "sxgbnaqw1");
                // TODO: Authorize the service object for a specific user (see Authorizing requests)

                DocumentEntry documentEntry = new DocumentEntry();
                documentEntry.Title.Text = MyPocketMoney;

                documentEntry.Categories.Add(DocumentEntry.SPREADSHEET_CATEGORY);

                documentsService.Insert(DocumentsListQuery.documentsBaseUri, documentEntry);

                SpreadsheetFeed spreadsheetFeed = service.Query(query);
                foreach (SpreadsheetEntry sheet in spreadsheetFeed.Entries)
                {
                    if (sheet.Title.Text == MyPocketMoney)
                    {
                        mainSpreadSheet = sheet;
                        WorksheetEntry timerangesWorkSheet = new WorksheetEntry();
                        timerangesWorkSheet.Title.Text = "TimeRanges";
                        timerangesWorkSheet.Cols = 10;
                        timerangesWorkSheet.Rows = 2000;
                        service.Insert(mainSpreadSheet.Worksheets, timerangesWorkSheet);
                        Timeranges = timerangesWorkSheet;

                        WorksheetEntry recordsWorkSheet = new WorksheetEntry();
                        recordsWorkSheet.Title.Text = "Records";
                        recordsWorkSheet.Cols = 10;
                        recordsWorkSheet.Rows = 2000;
                        service.Insert(mainSpreadSheet.Worksheets, recordsWorkSheet);
                        Records = recordsWorkSheet;
                        return sheet;

                    }
                }
            }



            return null;

        }
    }
}
