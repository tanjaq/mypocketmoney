using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using WpfApplication2.DomainObjects;
using WpfApplication2.Repos;
using WpfApplication2.utils;

namespace WpfApplication2.ViewModels
{
    public class RecordsViewModel : BaseViewModel
    {
        private Visibility _showDeleteButtons = Visibility.Collapsed;
        private Visibility _showTextBlocks = Visibility.Visible;
        private string _name;
        private string _amount;
        private bool _isPaid;
        private ObservableCollection<TimeRange> _timeRanges = new ObservableCollection<TimeRange>();

        public RecordsViewModel(params TimeRange[] timeRange)
        {
            if (timeRange.Length > 0) 
                CurrenTimeRange = timeRange[0];
            Add = new Command(OnAdd);
            Remove = new Command<Record>(OnRemove);
            Edit = new Command(OnEdit);
            SaveRecord = new Command<Record>(OnSaveRecord);
            Cancel = new Command<Record>(OnCancel);
            ExpandCollapse = new Command<TimeRange>(OnExpandCollapseAndLoad);
            AddTimeRange = new Command<TimeRange>(OnAddTimeRange);
        }

        private void OnAddTimeRange(TimeRange timeRange)
        {
            RegionManager.NavigateUsingViewModel<InitTimeRangeViewModel>(RegionNames.ContentRegion, timeRange.Id);
        }

        private void OnExpandCollapseAndLoad(TimeRange obj)
        {
            if (CurrenTimeRange != null)
            {
                CurrenTimeRange.Records.Clear();
                CurrenTimeRange.ExpandVisibility = Visibility.Collapsed;
            }
            CurrenTimeRange = obj;
            RecordsRepository.LoadRecords(obj.Records, obj);
            obj.ExpandVisibility = Visibility.Visible;
        }

        private void OnCancel(Record obj)
        {
            obj.Name = obj.oldname;
            obj.Amount = obj.oldAmount;
            obj.IsPaid = obj.oldPaid;
        }

        private void OnSaveRecord(Record obj)
        {
            obj.oldAmount = obj.Amount;
            obj.oldname = obj.Name;
            obj.oldPaid = obj.IsPaid;
            obj.TimeRangeId = CurrenTimeRange.Id;
            obj.PropertyChangedValidator();
            RecordsRepository.Save(obj);
            CurrenTimeRange.OnPropertyChanged("WillSpendAmount");
            CurrenTimeRange.OnPropertyChanged("CurrentAmount");
            CurrenTimeRange.OnPropertyChanged("SavedAmount");
        }

        private void OnEdit()
        {
            if (ShowDeleteButtons == Visibility.Collapsed)
            {
                ShowDeleteButtons = Visibility.Visible;
                ShowTextBlocks = Visibility.Collapsed;
            }
            else
            {
                ShowDeleteButtons = Visibility.Collapsed;
                ShowTextBlocks = Visibility.Visible;
            }
        }

        private void OnRemove(Record obj)
        {
            obj.IsRemoved = true;
            obj.Visibility = Visibility.Collapsed;
            RecordsRepository.Save(obj);
            CurrenTimeRange.Records.Remove(obj);
        }

        private void OnAdd()
        {
            if (String.IsNullOrEmpty(Name))
                return;
            decimal amount;
            if (!decimal.TryParse(Amount, out amount))
            {
                return;
            }
            var record = new Record(Name, amount, IsPaid,CurrenTimeRange.Id);
            CurrenTimeRange.Records.Add(record);
            RecordsRepository.Save(record);
            Name = "";
            Amount = "";
            IsPaid = false;
            CurrenTimeRange.OnPropertyChanged("WillSpendAmount");
            CurrenTimeRange.OnPropertyChanged("CurrentAmount");
            CurrenTimeRange.OnPropertyChanged("SavedAmount");
        }

        public Visibility ShowDeleteButtons
        {
            get { return _showDeleteButtons; }
            set
            {
                _showDeleteButtons = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public TimeRange CurrenTimeRange { get; set; }

        public string Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public bool IsPaid
        {
            get { return _isPaid; }
            set
            {
                _isPaid = value;
                OnPropertyChanged();
            }
        }

        public Command Edit { get; set; }
        public Command Add { get; set; }
        public Command<Record> SaveRecord { get; set; }
        public Command<Record> Cancel { get; set; }
        public Command<Record> Remove { get; set; }
        public Command<TimeRange> ExpandCollapse { get; set; }
        public Command<TimeRange> AddTimeRange { get; set; }

        public ObservableCollection<TimeRange> TimeRanges
        {
            get { return _timeRanges; }
            set { _timeRanges = value; }
        }

        public IRecordsRepository RecordsRepository { get { return Kernel.Get<IRecordsRepository>(); } }

        public Visibility ShowTextBlocks
        {
            get { return _showTextBlocks; }
            set
            {
                _showTextBlocks = value;
                OnPropertyChanged();
            }
        }

        public override void OnNavigationCompleted()
        {
            var timeranges = RecordsRepository.LoadLastTimeRanges();
            OrganizeTimeranges(timeranges);
            var lastTimeRange = TimeRanges.LastOrDefault();
            if (lastTimeRange != null)
            {
                if (CurrenTimeRange != null)
                    lastTimeRange = CurrenTimeRange;
                OnExpandCollapseAndLoad(lastTimeRange);
            }
            else
            {
                RegionManager.NavigateUsingViewModel<InitTimeRangeViewModel>(RegionNames.ContentRegion);
            }

            base.OnNavigationCompleted();
        }

        private void OrganizeTimeranges(IDictionary<long, TimeRange> timeRanges)
        {
            var lastTimerange = timeRanges.LastOrDefault().Value;
            if (lastTimerange != null)
            {
                TimeRanges.Add(lastTimerange);
                while (lastTimerange.NextTimeRangeId != 0)
                {
                    TimeRanges.Add(timeRanges[lastTimerange.NextTimeRangeId]);
                    lastTimerange = timeRanges[lastTimerange.NextTimeRangeId];
                }
                while (lastTimerange.PrevTimeRangeId != 0)
                {
                    TimeRanges.Insert(0, timeRanges[lastTimerange.PrevTimeRangeId]);
                    lastTimerange = timeRanges[lastTimerange.PrevTimeRangeId];
                }
            }
        }
    }
}
