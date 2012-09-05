using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MyPlaces.Model;
using System.Windows.Media.Imaging;

namespace MyPlaces.View
{
    public partial class MapItemContextView : UserControl
    {
        public enum ContextType
        {
            Pro, Con, Detail
        }

        private static readonly string PLUS_IMG = "/Resources/Images/ico_plus_smaller.png";
        private static readonly string MINUS_IMG = "/Resources/Images/ico_minus_smaller.png";
        private static readonly string DETAIL_IMG = "/Resources/Images/ico_pencil_smaller.png";


        private string mValue;
        private Detail mDetail;
        public ContextType Type { get; private set; }

        public MapItemContextView(object value, ContextType type)
        {
            InitializeComponent();
            if (type == ContextType.Detail)
                SetValue((Detail)value);
            else
                SetValue((string)value, type);
        }

        private void SetValue(string value, ContextType type)
        {
            Type = type;
            mValue = value;
            if(type == ContextType.Con)
                Icon.Source = new BitmapImage(new Uri(MINUS_IMG, UriKind.RelativeOrAbsolute));
            else if (type == ContextType.Pro)
                Icon.Source = new BitmapImage(new Uri(PLUS_IMG, UriKind.RelativeOrAbsolute));
            TextStack.Children.Remove(ContentBottom);
            ContentTop.Text = value;
        }

        private void SetValue(Detail value)
        {
            mDetail = value;
            Type = ContextType.Detail;
            Icon.Source = new BitmapImage(new Uri(DETAIL_IMG, UriKind.RelativeOrAbsolute));
            ContentTop.Text = value.What;
            ContentBottom.Text = value.Text;
        }

        public object GetValue()
        {
            if (Type == ContextType.Detail)
                return mDetail;
            else
                return mValue;
        }
    }
}
