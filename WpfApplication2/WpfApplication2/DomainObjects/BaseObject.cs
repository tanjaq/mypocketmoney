using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;
using WpfApplication2.Annotations;

namespace WpfApplication2.DomainObjects
{
    public abstract class BaseObject : INotifyPropertyChanged
    {
        public long Id { get; set; }

        public abstract string Tablename { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}