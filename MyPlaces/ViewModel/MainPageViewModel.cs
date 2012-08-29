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
        private ServerConnection mConnection;

        public MainPageViewModel(MainPage page)
        {
            mPage = page;
            init();
        }

        private void init()
        {
            mConnection = new ServerConnection();
            mConnection.GetStars(new DataAsyncCallback<List<Star>>((res) => { mPage.Dispatcher.BeginInvoke(new Action(() => OnLoadedStars(res.DataResult))); }));
        }

        public void OnLoadedStars(List<Star> data)
        {
            mStars = data;
            foreach(Star s in mStars)
            {
                Pushpin p = new Pushpin() { Location = new System.Device.Location.GeoCoordinate(s.Y, s.X) };
                mPage.Map.Children.Add(p);                
            }
        }
    }
}
