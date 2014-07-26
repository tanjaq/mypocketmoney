using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;
using WpfApplication2.Annotations;

namespace WpfApplication2.DomainObjects
{
    public abstract class BaseObject : INotifyPropertyChanged
    {
        public long Id { get; set; }

        public BaseObject()
        {

        }

        public abstract string Tablename { get; }

        public IDictionary<string, object> GetDataForSaving()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            foreach (var field in this.LocalFields)
            {
                var property = this.GetType().GetProperty(field);
                if (property == null)
                    throw new Exception("Invalid Property name");
                values.Add(field.ToLower(), property.GetValue(this, null));
            }

            return values;
        }

        public void SetValues(IDictionary<string, string> keyvalues)
        {
            foreach (var field in this.LocalFields)
            {
                var property = this.GetType().GetProperty(field);
                if (property == null)
                    throw new Exception("Invalid Property name");
                object value = keyvalues[field.ToLower()];
                if (property.PropertyType == typeof(long))
                {
                    value = Convert.ToInt64(value);
                }
                if (property.PropertyType == typeof(decimal))
                {
                    value = Convert.ToDecimal(value);
                }
                if (property.PropertyType == typeof(DateTime))
                {
                    value = Convert.ToDateTime(value);
                }
                if (property.PropertyType == typeof(bool))
                {
                    value = Convert.ToBoolean(value);
                }
                if (property.PropertyType == typeof(int))
                {
                    value = Convert.ToInt32(value);
                }
                property.SetValue(this, value, null);
            }
        }

        public abstract IEnumerable<string> LocalFields { get; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}