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
using System.Collections.Generic;
using MyPlaces.Model;
using MyPlaces.Server;
using Microsoft.Phone.Controls.Maps;

namespace MyPlaces.ViewModel
{
    public class MainPageViewModel
    {
        private MainPage mPage;
        private List<Star> mStars;
        private List<MapItem> mMapItems;
        private ServerConnection mConnection;

        public MainPageViewModel(MainPage page)
        {
            mPage = page;
            mPage.Loaded += new RoutedEventHandler((o,e) => init());
        }

        private void init()
        {
            mConnection = new ServerConnection();
            mConnection.GetStars(new DataAsyncCallback<List<Star>>((res) => { mPage.Dispatcher.BeginInvoke(new Action(() => OnLoadStars(res.DataResult))); }));
            mConnection.GetMapItems(new DataAsyncCallback<List<MapItem>>((res) => { mPage.Dispatcher.BeginInvoke(new Action(() => OnLoadMapItems(res.DataResult))); }));
        }

        public void OnLoadStars(List<Star> data)
        {
            mStars = data;
            foreach (Star s in data)
            {
                Pushpin p = new Pushpin() { Location = new System.Device.Location.GeoCoordinate(s.Y, s.X), Tag = s };                
                mPage.Map.Children.Add(p);                
            }
        }

        public void OnLoadMapItems(List<MapItem> data)
        {
            mMapItems = data;
            foreach (MapItem s in data)
            {
                Pushpin p = new Pushpin() { Location = new System.Device.Location.GeoCoordinate(s.Y, s.X), Tag = s };
                p.Content = new Rectangle(){Height = 5, Width =5, Stroke=new SolidColorBrush(Colors.Cyan),StrokeThickness = 3};
                mPage.Map.Children.Add(p);
                p.MouseLeftButtonUp += (o, e) => { MapItem mi = (MapItem)((Pushpin)o).Tag; MessageBox.Show(mi.Name); };
            }
        }
    }
}
