using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfApplication2.Services
{
    public class MyDispatcher : IDispatcher
    {
        public Dispatcher CurrentDispatcher { get; set; }

        public void Invoke(Action method)
        {
            CurrentDispatcher.Invoke(method);
        }
    }
}
