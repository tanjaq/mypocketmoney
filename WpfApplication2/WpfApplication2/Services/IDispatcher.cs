using System;
using System.Windows.Threading;

namespace WpfApplication2.Services
{
    public interface IDispatcher
    {
        Dispatcher CurrentDispatcher { get; set; }
        void Invoke(Action method);
    }
}
