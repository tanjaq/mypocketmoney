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
        private const string Sequences = "Sequences";
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
                        timerangesWorkSheet.Title.Text = TimeRange.CurrentTableName;
                        timerangesWorkSheet.Cols = (uint)TimeRange.Fields.Count;
                        timerangesWorkSheet.Rows = 1;
                        service.Insert(mainSpreadSheet.Worksheets, timerangesWorkSheet);



                        WorksheetEntry recordsWorkSheet = new WorksheetEntry();
                        recordsWorkSheet.Title.Text = Record.CurrentTableName;
                        recordsWorkSheet.Cols = (uint)Record.Fields.Count;
                        recordsWorkSheet.Rows = 1;
                        service.Insert(mainSpreadSheet.Worksheets, recordsWorkSheet);

                        WorksheetEntry sequences = new WorksheetEntry();
                        sequences.Title.Text = Sequences;
                        sequences.Cols = 2;
                        sequences.Rows = 10;
                        service.Insert(mainSpreadSheet.Worksheets, sequences);

                        foreach (WorksheetEntry worksheet in mainSpreadSheet.Worksheets.Entries)
                        {
                            worksheets.Add(worksheet.Title.Text, worksheet);
                        }
                        CellQuery cellQuery = new CellQuery(worksheets[Record.CurrentTableName].CellFeedLink);
                        CellFeed cellFeed = service.Query(cellQuery);

                        // Iterate through each cell, updating its value if necessary.

                        for (uint i = 1; i <= Record.Fields.Count; i++)
                        {
                            var field = Record.Fields[(int)i - 1].ToLower();
                            cellFeed.Insert(new CellEntry(1, i, field));
                        }

                        cellQuery = new CellQuery(worksheets[TimeRange.CurrentTableName].CellFeedLink);
                        cellFeed = service.Query(cellQuery);
                        for (uint i = 1; i <= TimeRange.Fields.Count; i++)
                        {
                            var field = TimeRange.Fields[(int)i - 1].ToLower();
                            cellFeed.Insert(new CellEntry(1, i, field));
                        }

                        cellQuery = new CellQuery(worksheets[Sequences].CellFeedLink);
                        cellFeed = service.Query(cellQuery);
                        cellFeed.Insert(new CellEntry(1, 1, "tablename"));
                        cellFeed.Insert(new CellEntry(1, 2, "indexvalue"));
                        cellFeed.Insert(new CellEntry(2, 1, Record.CurrentTableName));
                        cellFeed.Insert(new CellEntry(3, 1, TimeRange.CurrentTableName));
                        cellFeed.Insert(new CellEntry(2, 2, "0"));
                        cellFeed.Insert(new CellEntry(3, 2, "0"));


                        return sheet;

                    }
                }
            }



            return null;

        }

        public BaseObject InsertUpdate(BaseObject item)
        {


            ListEntry row = new ListEntry();
            if (item.Id == 0)
            {
                item.Id = GenerateID(item.Tablename);
            }

            var worksheet = worksheets[item.Tablename];
            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = service.Query(listQuery);

            foreach (var localField in item.GetDataForSaving())
            {
                var value = "";
                if (localField.Value != null)
                {
                    value = localField.Value.ToString();
                }
                ListEntry.Custom custom = new ListEntry.Custom()
                {
                    LocalName = localField.Key,
                    Value = value,
                };
                row.Elements.Add(custom);
            }
            service.Insert(listFeed, row);
            return item;
        }

        private long GenerateID(string tablename)
        {
            AtomLink listFeedLink = worksheets[Sequences].Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = service.Query(listQuery);
            foreach (ListEntry row in listFeed.Entries)
            {
                if (row.Elements[0].Value == tablename)
                {
                    var id = Convert.ToInt64(row.Elements[1].Value);
                    id++;
                    row.Elements[1].Value = id.ToString();
                    row.Update();
                    return id;
                }

            }
            return 0;
        }

        public T GetById<T>(long id) where T : BaseObject
        {
            var loadedValue = Activator.CreateInstance<T>();
            AtomLink listFeedLink = worksheets[loadedValue.Tablename].Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            listQuery.SpreadsheetQuery = "id = " + id;
            ListFeed listFeed = service.Query(listQuery);
            foreach (ListEntry row in listFeed.Entries)
            {
                var keyValues = new Dictionary<string, string>();
                foreach (ListEntry.Custom element in row.Elements)
                {
                    keyValues.Add(element.LocalName, element.Value);
                }
                loadedValue.SetValues(keyValues);
                return loadedValue;

            }
            return default(T);
        }
    }
}
