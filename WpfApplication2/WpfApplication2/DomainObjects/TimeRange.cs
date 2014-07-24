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
    [Table(Name = "Timeranges")]
    public class TimeRange : BaseObject
    {
        private ObservableCollection<Record> _records = new ObservableCollection<Record>();
        private Visibility _expandVisibility = Visibility.Collapsed;

        public TimeRange(long id)
        {
            Id = id;
        }

        [Column]
        public DateTime StartDate { get; set; }
        public DateTime? EndTime { get; set; }
        [Column]
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

        [Column]
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

        [Column]
        public long NextTimeRangeId { get; set; }
        [Column]
        public long PrevTimeRangeId { get; set; }

        
    }
}
