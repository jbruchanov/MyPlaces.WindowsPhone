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
using MyPlaces.View;
using MyPlaces.Server;
using MyPlaces.Model;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;

namespace MyPlaces.ViewModel
{
    public class MapItemDetailViewModel
    {
        private MapItemDetail mPage;
        private ServerConnection mServerConnection;
        private string mMapItemId;
        private MapItem mMapItem;

        private ApplicationBarIconButton mDeleteButton;
        private ApplicationBarIconButton mSaveButton;
        private ApplicationBarIconButton mSearchButton;
        private ApplicationBarIconButton mAddButton;

        private Pushpin mPushpin;

        public MapItemDetailViewModel(MapItemDetail page)
        {
            mPage = page;
            mPage.Loaded += (o, e) => { InitAppBar(); Init(); };
            
            
        }

        private void Init()
        {
            mServerConnection = new ServerConnection();
            IDictionary<string,string> queryString = mPage.NavigationContext.QueryString;
            bool quit = false;
            if (queryString.ContainsKey(App.MAP_ITEM_ID))
            {
                mMapItemId = mPage.NavigationContext.QueryString[App.MAP_ITEM_ID];
            }
            else if (queryString.ContainsKey(App.X) && queryString.ContainsKey(App.Y))
            {
                double cx = Convert.ToDouble(queryString[App.X]);
                double cy = Convert.ToDouble(queryString[App.Y]);
                SetMapItem(new MapItem { X = cx, Y = cy });
            }
            else
                quit = true;


            if (quit)
            {
                mPage.NavigationService.GoBack();
            }
            else
            {
                mServerConnection.GetMapItemTypes(new DataAsyncCallback<List<string>>((res) => { mPage.Dispatcher.BeginInvoke(() => OnDownloadMapItemTypes(res.DataResult)); }));
            }

            mPage.RootPivot.SelectionChanged += new SelectionChangedEventHandler(RootPivot_SelectionChanged);
            mPage.Map.MouseLeftButtonDown += new MouseButtonEventHandler(Map_MouseLeftButtonDown);
            mPage.Map.MouseLeftButtonUp += new MouseButtonEventHandler(OnMapClick);
        }

        private long mMouseLeftButtonDownTime;
        void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mMouseLeftButtonDownTime = DateTime.Now.Ticks;
        }

        void OnMapClick(object sender, MouseButtonEventArgs e)
        {
            long diff = DateTime.Now.Ticks - mMouseLeftButtonDownTime;
            if (diff < 1000000)
            {
                GeoCoordinate coord = mPage.Map.ViewportPointToLocation(e.GetPosition(mPage.Map));
                mMapItem.X = coord.Longitude;
                mMapItem.Y = coord.Latitude;
                mPushpin.Location = coord;
            }
        }

        public virtual void OnDownloadMapItemTypes(List<string> list)
        {
            mPage.lpType.ItemsSource = list;
            if(!String.IsNullOrEmpty(mMapItemId))
                mServerConnection.GetMapItem(mMapItemId, new DataAsyncCallback<MapItem>((r) => { mPage.Dispatcher.BeginInvoke(() => OnDownloadMapItem(r.DataResult)); }));
        }

        void RootPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mSearchButton.IsEnabled = mPage.RootPivot.SelectedIndex == 0;
            mAddButton.IsEnabled = mPage.RootPivot.SelectedIndex == 2;
        }

        private void InitAppBar()
        {
            if (mPage.ApplicationBar.Buttons.Count != 0)
                return;
            mDeleteButton = new ApplicationBarIconButton
	        {
                IconUri = new Uri("/Resources/Icons/AppBarWhite/appbar.delete.rest.png", UriKind.Relative),
	            Text = Resources.Labels.lblDelete       
	        };
            mDeleteButton.Click += new EventHandler(OnApplicationBarButtonClick);
            mPage.ApplicationBar.Buttons.Add(mDeleteButton);

            mSearchButton = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Resources/Icons/AppBarWhite/appbar.feature.search.rest.png", UriKind.RelativeOrAbsolute),
                Text = Resources.Labels.lblSearch
            };
            mSearchButton.Click += new EventHandler(OnApplicationBarButtonClick);
            mPage.ApplicationBar.Buttons.Add(mSearchButton);

            mAddButton = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Resources/Icons/AppBarWhite/appbar.add.rest.png", UriKind.RelativeOrAbsolute),
                Text = Resources.Labels.lblAdd,
                IsEnabled = false
            };
            mAddButton.Click += new EventHandler(OnApplicationBarButtonClick);
            mPage.ApplicationBar.Buttons.Add(mAddButton);


            mSaveButton = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Resources/Icons/AppBarWhite/appbar.save.rest.png", UriKind.Relative),
                Text = Resources.Labels.lblSave,
            };
            mSaveButton.Click += new EventHandler(OnApplicationBarButtonClick);
            mPage.ApplicationBar.Buttons.Add(mSaveButton);
            
        }

        public virtual void OnApplicationBarButtonClick(object sender, EventArgs e)
        {
            if (mAddButton == sender)
                OnAddClick();
            else if (mDeleteButton == sender)
                OnDeleteClick();
            else if (mSearchButton == sender)
                OnSearchClick();
            else if (mSaveButton == sender)
                OnSaveClick();
        }

        public virtual void OnSaveClick()
        {

        }

        public virtual void OnSearchClick()
        {

        }

        public virtual void OnDeleteClick()
        {

        }

        public virtual void OnAddClick()
        {

        }

        public void OnDownloadMapItem(Model.MapItem mapItem)
        {
            if (mapItem == null)
            {
                //notify and go backs
            }
            else
            {
                SetMapItem(mapItem);
            }
        }

        public void SetMapItem(MapItem mi)
        {
            mMapItem = mi;
            mPage.RootPivot.Title = mi.Name;
            mPage.DataContext = mi;
            if (mPushpin == null)
            {
                mPushpin = new Pushpin { Location = new GeoCoordinate { Longitude = mi.X, Latitude = mi.Y } };
                mPage.Map.Children.Add(mPushpin);
            }
            else
                mPushpin.Location = new GeoCoordinate { Longitude = mi.X, Latitude = mi.Y };
            
        }

        public MapItem GetMapItem()
        {
            return null;
        }
    }
}
