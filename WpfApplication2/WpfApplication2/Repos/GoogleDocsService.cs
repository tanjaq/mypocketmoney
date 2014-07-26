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
using WpfApplication2.DomainObjects;
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

        private Dictionary<string, WorksheetEntry> worksheets = new Dictionary<string, WorksheetEntry>();

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
                    foreach (WorksheetEntry worksheet in mainSpreadSheet.Worksheets.Entries)
                    {
                        worksheets.Add(worksheet.Title.Text, worksheet);
                    }
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
                        timerangesWorkSheet.Cols = 4;
                        timerangesWorkSheet.Rows = 1;
                        service.Insert(mainSpreadSheet.Worksheets, timerangesWorkSheet);


                        


                        WorksheetEntry recordsWorkSheet = new WorksheetEntry();
                        recordsWorkSheet.Title.Text = "Records";
                        recordsWorkSheet.Cols = 4;
                        recordsWorkSheet.Rows = 1;
                        service.Insert(mainSpreadSheet.Worksheets, recordsWorkSheet);

                        foreach (WorksheetEntry worksheet in mainSpreadSheet.Worksheets.Entries)
                        {
                            worksheets.Add(worksheet.Title.Text, worksheet);
                        }
                        CellQuery cellQuery = new CellQuery(worksheets[Record.CurrentTableName].CellFeedLink);
                        CellFeed cellFeed = service.Query(cellQuery);

                        // Iterate through each cell, updating its value if necessary.

                        for (uint i = 0; i < Record.Fields.Count; i++)
                        {
                            var field = Record.Fields[(int) i];
                            cellFeed.Insert(new CellEntry(0, i, field));
                        }

                        cellQuery = new CellQuery(worksheets[TimeRange.CurrentTableName].CellFeedLink);
                        cellFeed = service.Query(cellQuery);
                        for (uint i = 0; i < TimeRange.Fields.Count; i++)
                        {
                            var field = TimeRange.Fields[(int)i];
                            cellFeed.Insert(new CellEntry(0, i, field));
                        }
                       
                        
                        return sheet;

                    }
                }
            }



            return null;

        }

        public BaseObject InsertUpdate(BaseObject item)
        {
            var worksheet = worksheets[item.Tablename];
            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = service.Query(listQuery);

            // Create a local representation of the new row.
            ListEntry row = new ListEntry();
            row.Elements.Add(new ListEntry.Custom() { LocalName = "namee" , Value = "dsdg" });
            row.Elements.Add(new ListEntry.Custom() { LocalName = "id",Value = "Smit3h1" });
            row.Elements.Add(new ListEntry.Custom() { LocalName="value", Value = "246" });
            row.Elements.Add(new ListEntry.Custom() { LocalName = "value2",Value = "16" });

            // Send the new row to the API for insertion.
            service.Insert(listFeed, row);
            return item;
        }
    }
}
