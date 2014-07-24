using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Spreadsheets;
using WpfApplication2.DomainObjects;

namespace WpfApplication2.Repos
{
    public class RecordsRepository : IRecordsRepository
    {
        private SpreadsheetsService service;
        public RecordsRepository()
        {


        }
        public void LoadRecords(ObservableCollection<Record> records, TimeRange lastRecord)
        {

            //SpreadsheetQuery query = new SpreadsheetQuery();
            //SpreadsheetFeed feed = service.Query(query);
            //feed

            //var toDoItemsInDB = from Record todo in service.Records
            //                    where todo.TimeRangeId == lastRecord.Id
            //                    select todo;
            //foreach (var item in toDoItemsInDB)
            //{
            //    records.Add(item);
            //}
        }

        public BaseObject Save(BaseObject record)
        {
            //service.Records.InsertOnSubmit(record);
            //service.SubmitChanges();
            return record;
        }


        public IDictionary<long, TimeRange> LoadLastTimeRanges()
        {
            var timeranges = new Dictionary<long, TimeRange>();
            //var toDoItemsInDB = from TimeRange todo in service.Records
            //                    select todo;
            //foreach (var item in toDoItemsInDB)
            //{
            //    timeranges.Add(item.Id, item);
            //}
            return timeranges;
        }




    }
}
