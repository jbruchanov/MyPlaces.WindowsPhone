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
    public class MapItemContextItem
    {
        public ContextItemType Type { get; private set; }
        public Detail Detail { get; private set; }
        public string Value { get; private set; }

        public MapItemContextItem(ContextItemType type, string value)
        {
            Type = type;
            Value = value;
        }

        public MapItemContextItem(Detail detail)
        {
            Detail = detail;
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
    }
}
