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
        private static readonly string PLUS_IMG = "/Resources/Images/ico_plus_smaller.png";
        private static readonly string MINUS_IMG = "/Resources/Images/ico_minus_smaller.png";
        private static readonly string DETAIL_IMG = "/Resources/Images/ico_pencil_smaller.png";

        private MapItemContextItem mMapItemContextItem;
        public ContextItemType Type { get; private set; }

        public MapItemContextView(MapItemContextItem value)
        {
            InitializeComponent();

            mMapItemContextItem = value;
            if (value.Type == ContextItemType.Detail)
                SetValue(value.Detail);
            else
                SetValue(value.Value, value.Type);
        }

        private void SetValue(string value, ContextItemType type)
        {
            Type = type;
            if (type == ContextItemType.Con)
                Icon.Source = new BitmapImage(new Uri(MINUS_IMG, UriKind.RelativeOrAbsolute));
            else if (type == ContextItemType.Pro)
                Icon.Source = new BitmapImage(new Uri(PLUS_IMG, UriKind.RelativeOrAbsolute));
            TextStack.Children.Remove(ContentBottom);
            ContentTop.Text = value;
        }

        private void SetValue(Detail value)
        {
            Type = ContextItemType.Detail;
            Icon.Source = new BitmapImage(new Uri(DETAIL_IMG, UriKind.RelativeOrAbsolute));
            ContentTop.Text = value.What;
            ContentBottom.Text = value.Text;
        }

        public MapItemContextItem GetValue()
        {
            return mMapItemContextItem;
        }
    }
}
