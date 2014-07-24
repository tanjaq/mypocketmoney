using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.DomainObjects;

namespace WpfApplication2.Repos
{
    public class RepoCollection
    {
        public IList<Record> Records;
        public IList<TimeRange> TimeRanges;
    }
}
