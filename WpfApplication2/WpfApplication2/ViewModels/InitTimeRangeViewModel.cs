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
    public class InitTimeRangeViewModel : BaseViewModel
    {


        public InitTimeRangeViewModel(params TimeRange[] ids)
        {
            if (ids.Length > 0)
            {
                currentTimerange = ids[0];
            }
            Continue = new Command(OnContinue);
        }

        private TimeRange currentTimerange;

        private void OnContinue()
        {
            decimal amount;

            if (!decimal.TryParse(CurrentAmount, out amount))
                return;
            if (string.IsNullOrEmpty(Name))
                return;
            var timerange = new TimeRange(0) { Name = Name, InitialAmount = amount, PrevTimeRangeId = currentTimerange.Id };
            RecordsRepository.Save(timerange);
            RegionManager.NavigateUsingViewModel<RecordsViewModel>(RegionNames.ContentRegion, timerange);
        }
        public IRecordsRepository RecordsRepository { get { return Kernel.Get<IRecordsRepository>(); } }

        public Command Continue { get; set; }

        public string Name { get; set; }

        public string CurrentAmount { get; set; }

        public override void OnNavigationCompleted()
        {
            base.OnNavigationCompleted();
        }
    }
}
