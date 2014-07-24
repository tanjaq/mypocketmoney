using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication2.utils
{
    public class MyContentRegion : ContentControl
    {



        public void Close()
        {
            var oldContent = (UserControl)this.Content;
            this.Visibility = Visibility.Collapsed;
            if (oldContent != null)
            {
                if (oldContent.DataContext != null)
                {
                    var baseViewModel = oldContent.DataContext as IBaseViewModel;
                    if (baseViewModel != null)
                    {
                        baseViewModel.Close();
                    }
                }
            }
            
        }
    }
}
