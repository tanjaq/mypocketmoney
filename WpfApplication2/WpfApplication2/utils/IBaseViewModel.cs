using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication2.utils
{
    public interface IBaseViewModel : INotifyPropertyChanged, IClosable
    {
        void Close();
        void OnNavigationCompleted();
        UserControl View { get; set; }
        bool IsReady { get; }
        Window ViewWindow { get; set; }
        void ShowMessage(string msg1);
        void ShowError(string obj, EventHandler okClick = null, bool bCreateButtonEvent = false, int iAddCounterSeconds = 0);
    }
}
