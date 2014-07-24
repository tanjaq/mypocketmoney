using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication2.utils
{
    public class RegionManager : IRegionManager
    {

        private static Dictionary<string, MyContentRegion> Regions = new Dictionary<string, MyContentRegion>();

        public static DependencyProperty RegionNameProperty = DependencyProperty.RegisterAttached("RegionName", typeof(string), typeof(RegionManager), new FrameworkPropertyMetadata(IsNameChanged));


        private static Dictionary<string, IList<ViewToViewModel>> RegionHistory = new Dictionary<string, IList<ViewToViewModel>>();
        private static Dictionary<string, ViewToViewModel> RegionToViewModel = new Dictionary<string, ViewToViewModel>();




        public static string GetRegionName(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (string)element.GetValue(RegionNameProperty);
        }

        public static void SetRegionName(DependencyObject element, string value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            Regions[value] = (MyContentRegion)element;

            element.SetValue(RegionNameProperty, value);
        }

        private static void IsNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var region = (MyContentRegion)d;

            Regions[(string)e.NewValue] = region;
            RegionHistory[(string)e.NewValue] = new List<ViewToViewModel>();

        }

        public T NavigateUsingViewModel<T>(string regionName, params object[] args)
        {
            T returnValue = default(T);
            if (Regions.ContainsKey(regionName))
            {

                {
                    try
                    {
                        var region = Regions[regionName];

                        ClearForwardHistory(regionName);
                        var modelName = typeof(T).Name;



                        var oldControl = (UserControl)region.Content;

                        if (oldControl != null)
                        {
                            var baseViewModel = oldControl.DataContext as IBaseViewModel;
                            if (baseViewModel != null)
                            {
                                oldControl.DataContext = null;
                                baseViewModel.Close();
                            }
                        }
                        region.Visibility = Visibility.Visible;

                        var control = FindViewByViewModel<T>();
                        var viewModel = Activator.CreateInstance(typeof(T), args);
                        region.Content = control;
                        control.DataContext = viewModel;
                        returnValue = (T)viewModel;
                        if (viewModel is IBaseViewModel)
                            ((IBaseViewModel)viewModel).View = control;
                        var vievToViewModel = new ViewToViewModel(control, (IBaseViewModel)viewModel, region, args);
                        RegionHistory[regionName].Add(vievToViewModel);
                        RegionToViewModel[regionName] = vievToViewModel;

                    }
                    catch (Exception ex)
                    {
                        //Log.Error(ex.Message, ex);
                    }
                }


            }
            else
            {
                throw new Exception("Invalid region name.");
            }

            return returnValue;
        }




        public void CloseAllViewsInRegion(string regionName)
        {
            if (Regions.ContainsKey(regionName))
            {

                {
                    for (int i = 0; i < RegionHistory[regionName].Count; i++)
                    {
                        ViewToViewModel vtVm = RegionHistory[regionName].ElementAt(i) as ViewToViewModel;
                        var control = vtVm.View;
                        var vievModel = control.DataContext as IBaseViewModel;
                        if (vievModel != null)
                        {
                            control.DataContext = null;
                            vievModel.Close();
                        }
                    }
                }
            }
        }


        private UserControl FindViewByViewModel<T>()
        {
            var type = typeof(T);
            string typeName = type.ToString();
            string[] strArray = typeName.Split('.');
            var shortViewName = strArray.Last().Replace("ViewModel", "View");
            var viewName = typeName.Replace("ViewModels", "Views").Replace(strArray.Last(), shortViewName);


            var control = Activator.CreateInstance(type.Assembly.FullName.Split(',')[0], viewName);
            return (UserControl)control.Unwrap();


        }
        public Window FindWindowByViewModel<T>(bool init)
        {
            var type = typeof(T);
            string typeName = type.ToString();
            string[] strArray = typeName.Split('.');
            var shortViewName = strArray.Last().Replace("ViewModel", "Window");
            var viewName = typeName.Replace("ViewModels", "Views").Replace(strArray.Last(), shortViewName);


            Window window = null;
            var control = Activator.CreateInstance(type.Assembly.FullName.Split(',')[0], viewName);
            window = (Window)control.Unwrap();


            var vievModel = Activator.CreateInstance(typeof(T));
            window.DataContext = vievModel;
            if (vievModel is IBaseViewModel)
                ((IBaseViewModel)vievModel).ViewWindow = window;

            if (init)
            {
                window.Visibility = Visibility.Collapsed;
                window.Show();
                window.Hide();
                window.Visibility = Visibility.Visible;
            }
            if (Debugger.IsAttached)
                window.Topmost = false;
            return window;

        }


        public void NavigateForvard(string regionName)
        {
            if (Regions.ContainsKey(regionName))
            {
                {
                    var region = Regions[regionName];
                    int currentIndex = 0;
                    for (int i = RegionHistory[regionName].Count - 1; i >= 0; i--)
                    {
                        var viewToVieModel = RegionHistory[regionName][i];
                        if (Equals(viewToVieModel.View, Regions[regionName].Content))
                        {
                            currentIndex = i;
                        }
                    }
                    if (currentIndex < RegionHistory[regionName].Count - 1)
                    {
                        var oldControl = (UserControl)region.Content;
                        if (oldControl != null)
                        {
                            var baseViewModel = oldControl.DataContext as IBaseViewModel;
                            if (baseViewModel != null)
                            {
                                oldControl.DataContext = null;
                                baseViewModel.Close();
                            }
                        }


                        {
                            region.Visibility = Visibility.Collapsed;

                            RegionHistory[regionName][currentIndex + 1].Region.Visibility = Visibility.Visible;
                            var view = RegionHistory[regionName][currentIndex + 1].View;
                            if (RegionHistory[regionName][currentIndex + 1].Region.Content != null)
                            {
                                var baseViewModel = ((UserControl)RegionHistory[regionName][currentIndex + 1].Region.Content).DataContext as IBaseViewModel;
                                if (baseViewModel != null)
                                {
                                    baseViewModel.OnNavigationCompleted();
                                }
                                var vievToViewModel = new ViewToViewModel(view, baseViewModel, null);
                                RegionToViewModel[regionName] = vievToViewModel;

                            }


                        }

                    }
                }
            }
            else
            {
                throw new Exception("Invalid region name.");
            }
        }


        public object NavigatBack(string regionName)
        {
            object returnModel = null;
            if (Regions.ContainsKey(regionName))
            {
                {
                    var region = Regions[regionName];
                    int currentIndex = GetCurrentIndex(regionName);
                    if (currentIndex > 0)
                    {
                        var oldControl = (UserControl)region.Content;
                        if (oldControl != null)
                        {
                            var baseViewModel = oldControl.DataContext as IBaseViewModel;
                            if (baseViewModel != null)
                            {
                                oldControl.DataContext = null;
                                baseViewModel.Close();
                            }
                        }

                        {
                            region.Visibility = Visibility.Collapsed;

                            RegionHistory[regionName][currentIndex - 1].Region.Visibility = Visibility.Visible;
                            var view = RegionHistory[regionName][currentIndex - 1].View;
                            if (RegionHistory[regionName][currentIndex - 1].Region.Content != null)
                            {
                                var baseViewModel = ((UserControl)RegionHistory[regionName][currentIndex - 1].Region.Content).DataContext as IBaseViewModel;
                                if (baseViewModel != null)
                                {
                                    baseViewModel.OnNavigationCompleted();
                                }
                                var vievToViewModel = new ViewToViewModel(view, baseViewModel, null);
                                RegionToViewModel[regionName] = vievToViewModel;

                            }


                        }
                    }
                }
            }
            else
            {
                throw new Exception("Invalid region name.");
            }
            return returnModel;
        }

        private static int GetCurrentIndex(string regionName)
        {
            int currentIndex = 0;
            for (int i = RegionHistory[regionName].Count - 1; i >= 0; i--)
            {
                var viewToVieModel = RegionHistory[regionName][i];
                if (Equals(viewToVieModel.View, Regions[regionName].Content))
                {
                    currentIndex = i;
                }
            }
            return currentIndex;
        }

        public void ClearForwardHistory(string regionName)
        {
            if (Regions.ContainsKey(regionName))
            {
                {
                    int currentIndex = 0;
                    for (int i = RegionHistory[regionName].Count - 1; i >= 0; i--)
                    {
                        var viewToVieModel = RegionHistory[regionName][i];
                        if (Equals(viewToVieModel.View, Regions[regionName].Content))
                        {
                            currentIndex = i;
                        }
                    }
                    while (currentIndex < RegionHistory[regionName].Count - 1)
                    {
                        RegionHistory[regionName].RemoveAt(RegionHistory[regionName].Count - 1);
                    }
                }
            }
            else
            {
                throw new Exception("Invalid region name.");
            }
        }

        public IBaseViewModel CurrentViewModelInRegion(string regionName)
        {

            if (RegionToViewModel.ContainsKey(regionName))
                return RegionToViewModel[regionName].ViewModel;

            return null;
        }

        public void ClearHistory(string regionName)
        {

            if (Regions.ContainsKey(regionName))
            {
                RegionHistory[regionName].Clear();
            }
            else
            {
                throw new Exception("Invalid region name.");
            }
        }

        public Type CurrentViewModelType(string regionName)
        {
            if (RegionToViewModel.ContainsKey(regionName))
                return RegionToViewModel[regionName].ViewModelType;
            return null;
        }

        public Type PreviousViewModelType(string regionName)
        {
            try
            {
                int currentIndex = GetCurrentIndex(regionName);
                if (currentIndex > 0) return RegionHistory[regionName][currentIndex - 1].ViewModelType;
            }
            catch (Exception)
            { }
            return null;
        }

    }

    internal class ViewToViewModel
    {
        public ViewToViewModel(UserControl control, IBaseViewModel type, MyContentRegion region, params object[] args)
        {
            View = control;
            ViewModel = type;
            Region = region;
            if (args != null)
                Args = args;
        }

        public object[] Args { get; set; }

        public Type ViewModelType { get { return ViewModel.GetType(); } }

        public UserControl View { get; set; }
        public MyContentRegion Region { get; set; }

        public IBaseViewModel ViewModel { get; set; }
    }
}
