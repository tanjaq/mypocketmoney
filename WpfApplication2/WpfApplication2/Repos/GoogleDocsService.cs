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

                insertFile(MyPocketMoney, MyPocketMoney, null, "application/vnd.google-apps.spreadsheet", MyPocketMoney);

                return GetSetSpreadSheet();
            }

            return new SpreadsheetFeed(new Uri(mainSpreadsheetEntryuri), service);

        }
        public static File insertFile(String title, String description, String parentId, String mimeType, String filename)
        {

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "403400217676-pcs2mvcgegbgsvjnc3fld9edafq8ppdu.apps.googleusercontent.com",
                    ClientSecret = "JL622ryC3smnyrfvh08jWESD",
                },
                new[] { DriveService.Scope.Drive },
                "user",
                CancellationToken.None).Result;

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Sample",
            });            // File's metadata.
            File body = new File();
            body.Title = title;
            body.Description = description;
            body.MimeType = mimeType;

            // Set the parent folder.
            if (!String.IsNullOrEmpty(parentId))
            {
                body.Parents = new List<ParentReference>() { new ParentReference() { Id = parentId } };
            }

            // File's content.
            byte[] byteArray = System.IO.File.ReadAllBytes(filename);
            MemoryStream stream = new MemoryStream(byteArray);
            try
            {
                FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, mimeType);
                request.Upload();

                File file = request.ResponseBody;

                // Uncomment the following line to print the File ID.
                // Console.WriteLine("File ID: " + file.Id);

                return file;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }
    }
}
