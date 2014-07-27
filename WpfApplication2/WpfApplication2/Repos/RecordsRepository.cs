using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Google.GData.Spreadsheets;
using WpfApplication2.DomainObjects;

namespace WpfApplication2.Repos
{
    public class RecordsRepository : IRecordsRepository
    {
        private GoogleDocsService service;
        public RecordsRepository()
        {
            service = new GoogleDocsService();
            service.GetSetSpreadSheet();

        }
        public void LoadRecords(ObservableCollection<Record> records, TimeRange timeRange)
        {
            var loadedrecords = service.GetByQuery<Record>("TimeRangeId = {0}", timeRange.Id);
            foreach (var loadedrecord in loadedrecords)
            {
                if (records.All(x => x.Id != loadedrecord.Id))
                {
                    if (!loadedrecord.IsPaid)
                    {
                        int i = 0;
                        for (; i < records.Count; i++)
                        {
                            var record = records[i];
                            if (record.IsPaid && !loadedrecord.IsPaid)
                            {
                                break;
                            }
                        }
                        records.Insert(i, loadedrecord);

                    }
                    else
                    {
                        records.Add(loadedrecord);
                    }
                }
            }
            for (int i = 0; i < records.Count; i++)
            {
                if (loadedrecords.All(x => x.Id != records[i].Id))
                {
                    records.RemoveAt(i);
                    i--;
                }
            }

        }

        public BaseObject Save(BaseObject record)
        {
            return service.InsertUpdate(record);
        }


        public IDictionary<long, TimeRange> LoadLastTimeRanges()
        {
            var timeranges = new Dictionary<long, TimeRange>();

            var loadedTimeRanges = service.GetByQuery<TimeRange>("");
            foreach (var loadedTimeRange in loadedTimeRanges)
            {
                timeranges.Add(loadedTimeRange.Id, loadedTimeRange);
            }
            return timeranges;
        }




    }
}
