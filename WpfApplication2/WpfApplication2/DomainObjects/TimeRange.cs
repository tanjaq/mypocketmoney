using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication2.Annotations;

namespace WpfApplication2.DomainObjects
{
    public class TimeRange : BaseObject
    {
        private ObservableCollection<Record> _records = new ObservableCollection<Record>();
        private Visibility _expandVisibility = Visibility.Collapsed;

        public TimeRange()
        {
            
        }
        public TimeRange(long id)
        {
            Id = id;
        }

        public DateTime StartDate { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal InitialAmount { get; set; }
        public Visibility ExpandVisibility
        {
            get { return _expandVisibility; }
            set
            {
                if (value == _expandVisibility) return;
                _expandVisibility = value;
                OnPropertyChanged();
            }
        }

        public string Name { get; set; }
        public ObservableCollection<Record> Records
        {
            get { return _records; }
            set { _records = value; }
        }

        public decimal CurrentAmount
        {
            get
            {
                return InitialAmount - Records.Where(x => x.IsPaid).Sum(x => x.Amount);
            }
        }

        public decimal WillSpendAmount
        {
            get
            {
                return Records.Where(x => x.IsPaid == false).Sum(x => x.Amount);
            }
        }

        public decimal SavedAmount
        {
            get
            {
                return CurrentAmount - WillSpendAmount;
            }
        }

        public long NextTimeRangeId { get; set; }
        public long PrevTimeRangeId { get; set; }


        public static string CurrentTableName
        {
            get { return "TimeRanges"; }
        }

        public static IList<string> Fields
        {
            get
            {
                return new[]
                {
                    "Id",
                    "NextTimeRangeId",
                    "PrevTimeRangeId",
                    "InitialAmount",
                    "StartDate",
                    "EndTime",
                    "Name",
                };
            }
            
        }

        public override string Tablename
        {
            get { return CurrentTableName; }
        }

        public override IEnumerable<string> LocalFields
        {
            get { return Fields; }
        }
    }
}
