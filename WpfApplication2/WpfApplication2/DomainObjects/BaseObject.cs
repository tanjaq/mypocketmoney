using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;
using WpfApplication2.Annotations;

namespace WpfApplication2.DomainObjects
{
    public abstract class BaseObject : INotifyPropertyChanged
    {
        [Column(IsPrimaryKey = true ,IsDbGenerated = true)]
        public long Id { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}