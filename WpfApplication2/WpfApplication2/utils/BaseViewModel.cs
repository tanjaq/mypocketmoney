using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Ninject;
using WpfApplication2.Services;

namespace WpfApplication2.utils
{
    public class BaseViewModel : IBaseViewModel
    {
        public static IKernel Kernel = new StandardKernel();
        public BaseViewModel()
        {
        }

        public IRegionManager RegionManager
        {
            get { return Kernel.Get<IRegionManager>(); }
        }


        public IDispatcher Dispatcher
        {
            get { return Kernel.Get<IDispatcher>(); }
        }

        public IMediator Mediator
        {
            get { return Kernel.Get<IMediator>(); }
        }

        public void ShowMessage(string obj)
        {

        }


        public void ShowError(string obj, EventHandler okClick = null, bool bCreateButtonEvent = false, int iAddCounterSeconds = 0)
        {

        }

        public UserControl View
        {
            get { return _view; }
            set
            {
                _view = value;
                View.Loaded += View_Loaded;
            }
        }

        public bool IsReady { get; private set; }


        void View_Loaded(object sender, RoutedEventArgs e)
        {
            OnNavigationCompleted();
        }
        public virtual void OnNavigationCompleted()
        {
            IsClosed = false;


            //Mediator.ApplyRegistration();
            IsReady = true;
            if (View != null)
                View.Loaded -= View_Loaded;
            if (ViewWindow != null)
            {
                _viewWindow.Loaded -= View_Loaded;

            }
        }




        public Window ViewWindow
        {
            get { return _viewWindow; }
            set
            {
                _viewWindow = value;
                _viewWindow.Loaded += View_Loaded;
                _viewWindow.Closed += _viewWindow_Closed;
            }
        }

        void _viewWindow_Closed(object sender, EventArgs e)
        {
            if (ViewWindow != null)
            {
                _viewWindow.Closed -= _viewWindow_Closed;
            }
            Close();
        }


        public virtual void Close()
        {

            Mediator.UnregisterRecipientAndIgnoreTags(this);
            if (ViewWindow != null)
                ViewWindow.Close();
            IsClosed = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));

            }
            catch (Exception e)
            {
                if (Debugger.IsAttached)
                    throw;

                //Log.Error(e.Message);

            }
        }

        private UserControl _view;
        private Window _viewWindow;




        public bool IsClosed { get; set; }
    }
}
