using System;
using System.Windows;

namespace WpfApplication2.utils
{
    public interface IRegionManager
    {
        Type CurrentViewModelType(string contentRegion);
        Type PreviousViewModelType(string regionName);
        IBaseViewModel CurrentViewModelInRegion(string contentRegion);
        void ClearHistory(string contentRegion);
        T NavigateUsingViewModel<T>(string contentRegion, params object[] args);
        Window FindWindowByViewModel<T>(bool init = true);
        object NavigatBack(string contentRegion);
        void NavigateForvard(string usermanagementContentRegion);
        void CloseAllViewsInRegion(string regionName);
        void ClearForwardHistory(string regionName);
    }
}