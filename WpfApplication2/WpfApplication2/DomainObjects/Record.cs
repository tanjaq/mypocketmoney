using System;
using System.Collections;
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
    public class Record : BaseObject
    {
        private Visibility _visibility = Visibility.Visible;
        private Visibility _saveButtonVisibility = Visibility.Collapsed;
        private ObservableCollection<Record> _childs = new ObservableCollection<Record>();
        private decimal _amount;
        public string oldname;
        public decimal oldAmount;
        public bool oldPaid;
        private string _name;
        private bool _isPaid;

        public Record()
        { }

        public Record(string name, decimal amount, bool payd, long timerangeId)
        {
            oldAmount = _amount = amount;
            oldname = _name = name;
            oldPaid = _isPaid = payd;
            TimeRangeId = timerangeId;
        }

        public long TimeRangeId { get; set; }


        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (LoadingFromDatabase)
                    oldname = value;

                PropertyChangedValidator();
                OnPropertyChanged();
            }
        }

        public void PropertyChangedValidator()
        {
            if (Amount == oldAmount && Name == oldname && oldPaid == IsPaid)
            {
                SaveButtonVisibility = Visibility.Collapsed;
            }
            else
            {
                SaveButtonVisibility = Visibility.Visible;
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                if (LoadingFromDatabase)
                    oldAmount = value;
                PropertyChangedValidator();
                OnPropertyChanged();

            }
        }

        public bool IsPaid
        {
            get { return _isPaid; }
            set
            {
                if (value.Equals(_isPaid)) return;
                _isPaid = value;
                if (LoadingFromDatabase)
                    oldPaid = value;

                OnPropertyChanged();
                PropertyChangedValidator();
            }
        }

        public DateTime PaidTime { get; set; }

        public Record Parent { get; set; }
        public long ParentId { get; set; }

        public bool IsRemoved { get; set; }

        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Record> Childs
        {
            get { return _childs; }
            set { _childs = value; }
        }

        public Visibility SaveButtonVisibility
        {
            get { return _saveButtonVisibility; }
            set
            {
                if (value == _saveButtonVisibility) return;
                _saveButtonVisibility = value;
                OnPropertyChanged();
            }
        }



        public static string CurrentTableName
        {
            get { return "Records"; }
        }

        public static IList<string> Fields
        {
            get
            {
                return new List<string>()
                {
                    "Id",
                    "Name",
                    "PaidTime",
                    "Amount",
                    "IsPaid",
                    "ParentId",
                    "TimeRangeId",
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
