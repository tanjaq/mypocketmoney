using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.DomainObjects;

namespace WpfApplication2.Repos
{
    public interface IRecordsRepository
    {
        void LoadRecords(ObservableCollection<Record> records, TimeRange lastRecord);
        BaseObject Save(BaseObject record);
        IDictionary<long, TimeRange> LoadLastTimeRanges();
    }
}
