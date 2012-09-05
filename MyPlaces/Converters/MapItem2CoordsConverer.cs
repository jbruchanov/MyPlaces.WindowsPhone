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
using System.Windows.Data;
using MyPlaces.Model;
using System.Device.Location;

namespace MyPlaces.Converters
{
    public class MapItem2CoordsConverer : IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MapItem mi = value as MapItem;
            return new GeoCoordinate { Longitude = mi.X, Latitude = mi.Y };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
