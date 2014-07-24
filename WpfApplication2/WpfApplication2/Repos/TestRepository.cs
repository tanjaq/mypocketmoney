using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.DomainObjects;

namespace WpfApplication2.Repos
{
    public class TestRepository : IRecordsRepository
    {

        public TestRepository()
        {
            timeRanges.Add(2, new TimeRange(2) { Name = "1", InitialAmount = 1000, StartDate = new DateTime(2014, 12, 12), NextTimeRangeId = 3, PrevTimeRangeId = 1 });
            timeRanges.Add(1, new TimeRange(1) { Name = "test1", InitialAmount = 1300, StartDate = new DateTime(2014, 2, 12), NextTimeRangeId = 2 });
            timeRanges.Add(3, new TimeRange(3) { Name = "janvar", InitialAmount = 1200, StartDate = new DateTime(2014, 1, 12), PrevTimeRangeId = 2 });
        }
        public void LoadRecords(ObservableCollection<Record> records, TimeRange lastRecord)
        {
            records.Add(new Record("1", 1, false));
            records.Add(new Record("mashina", 1, false));
            records.Add(new Record("mashina lising", 1, false));
            records.Add(new Record("mashina lising bolshoj", 100, false));
            records.Add(new Record("mashina lising bolshoj strahovka", 1000, false));
            records.Add(new Record("mashina lising", 10.55m, true));
            records.Add(new Record("mashina lising", 10.55009m, false));
            records.Add(new Record("mashina lising", 10.55009m, true));
        }

        public BaseObject Save(BaseObject record)
        {
            if (record is TimeRange)
                timeRanges.Add(record.Id, (TimeRange)record);
            return record;
        }

        Dictionary<long, TimeRange> timeRanges = new Dictionary<long, TimeRange>();

        public IDictionary<long, TimeRange> LoadLastTimeRanges()
        {

            return timeRanges;
        }


        public void LoadLastTimeRanges(ObservableCollection<TimeRange> timeRanges)
        {
        }

    }
}
