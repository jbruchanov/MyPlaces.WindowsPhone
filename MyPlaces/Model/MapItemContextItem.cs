using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MyPlaces.Dialogs;
using System.ComponentModel;

namespace MyPlaces.Model
{
    public class MapItemContextItem : INotifyPropertyChanged
    {
        public ContextItemType Type { get; private set; }
        private Detail mDetail;
        private string mValue;

        public Detail Detail { get { return mDetail; } set { mDetail = value; NotifyChange("Detail"); } }
        public string Value { get { return mValue; } set { mValue = value; NotifyChange("Value"); } }

        public MapItemContextItem(ContextItemType type, string value)
        {
            Type = type;
            Value = value;
        }

        public MapItemContextItem(Detail detail)
        {
            Detail = detail;
            Detail.PropertyChanged += (o, e) => NotifyChange("Detail");
            Type = ContextItemType.Detail;
        }

        public ModalDialog CreateDialog()
        {
            ModalDialog result;
            if (Type == ContextItemType.Detail)
            {
                DetailContextDialog dcd = new DetailContextDialog(Detail);
                result = dcd;
            }
            else
            {
                SimpleContextDialog scd = new SimpleContextDialog(Type, Value);
                result = scd;
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyChange(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
