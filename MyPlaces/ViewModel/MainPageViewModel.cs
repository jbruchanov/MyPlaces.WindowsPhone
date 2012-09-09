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
using MyPlaces.Dialogs;
using System.Device.Location;
using System.Threading;
using MyPlaces.Resources;

namespace MyPlaces.ViewModel
{
    public class MainPageViewModel
    {
        private MainPage mPage;
        private List<Star> mStars;
        private List<MapItem> mMapItems;
        private ServerConnection mConnection;
        private MapLayer mStarsLayer;
        private MapLayer mItemsLayer;
        private Dialog mDialog;

        private GeoCoordinateWatcher mGeoWatcher;
        private Pushpin mMyLocation;
        private bool mIsInitialized = false;

        private enum UsageState
        {
            Default, AddingStar, AddingMapItem
        }
        private UsageState mState = UsageState.Default;

        public MainPageViewModel(MainPage page)
        {
            mPage = page;
            mPage.Loaded += new RoutedEventHandler((o,e) => Init());
            mPage.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(OnBackPress);
        }

        void OnBackPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mState = UsageState.Default;
            if (mDialog != null)
            {
                e.Cancel = mDialog.Hide();
                mDialog = null;
                if(e.Cancel)
                    return;
            }
            if (mPage.MapItemPreview.IsVisible)
            {
                mPage.MapItemPreview.Hide();
                e.Cancel = true;
                return;
            }
            
        }

        private void Init()
        {
            if (!mIsInitialized)
            {
                mStarsLayer = new MapLayer();
                mPage.Map.Children.Add(mStarsLayer);
                mItemsLayer = new MapLayer();
                mPage.Map.Children.Add(mItemsLayer);
                mConnection = new ServerConnection();
                mPage.MapItemPreview.OpenDetailClick += new EventHandler<DataEventArgs<MapItem>>(MapItemPreview_OpenDetailClick);
                mPage.Map.MouseLeftButtonUp += new MouseButtonEventHandler(OnMapClick);
                mPage.AddButton.Click += new RoutedEventHandler(OnAddClick);
                mIsInitialized = true;
            }
            
            mConnection.GetStars(new DataAsyncCallback<List<Star>>((res) => { mPage.Dispatcher.BeginInvoke(new Action(() => OnLoadStars(res.DataResult))); }));
            mConnection.GetMapItems(new DataAsyncCallback<List<MapItem>>((res) => { mPage.Dispatcher.BeginInvoke(new Action(() => OnLoadMapItems(res.DataResult))); }));
            
            InitGeoLocation();
            
        }

        private void InitGeoLocation()
        {
            mPage.MyPosition.Opacity = 0;
            GeoCoordinateWatcher gw = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            gw.StatusChanged += (o,e) =>
                {
                    if (e.Status == GeoPositionStatus.Disabled)
                    {
                        if (gw.Permission == GeoPositionPermission.Denied)
                        {
                            //no visibility
                        }
                        gw.Stop();
                    }
                    else if (e.Status == GeoPositionStatus.Ready)
                    {
                        mPage.MyPosition.Click += new RoutedEventHandler(MyPosition_Click);
                        mPage.MyPosition.Opacity = 100;
                        gw.Stop();
                    }
                    
                };
            gw.Start();
        }

        
        private void MyPosition_Click(object sender, RoutedEventArgs e)
        {
            if (mGeoWatcher == null)
            {
                mGeoWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                mGeoWatcher.MovementThreshold = 20;
                mGeoWatcher.PositionChanged += (o, args) =>
                    {
                        if (mMyLocation != null)
                            mPage.Map.Children.Remove(mMyLocation);
                        mPage.Map.Center = args.Position.Location;
                        mPage.Map.ZoomLevel = 17;
                        mMyLocation = new Pushpin { Location = args.Position.Location };
                        mMyLocation.MouseLeftButtonDown += (src, eargs) => { mPage.Map.Children.Remove((UIElement)src); };
                        mMyLocation.Foreground = new SolidColorBrush(Colors.Green);
                        mPage.Map.Children.Add(mMyLocation);
                        mGeoWatcher.Stop();
                        mGeoWatcher = null;
                    };
                mGeoWatcher.Start();
            }
            
        }

        
        protected void OnAddClick(object sender, RoutedEventArgs e)
        {
            AddNewItemDialog anid = new AddNewItemDialog(mPage.AddButton);
            anid.Click += (o, eargs) =>
                {
                    App.ShowToast(Labels.lblClickToMapForPosition);
                    if (o == anid.MapItem)
                    {
                        mState = UsageState.AddingMapItem;
                    }
                    else if (o == anid.Star)
                    {
                        mState = UsageState.AddingStar;
                    }
                };
            ShowDialog(anid);
        }

        protected void ShowDialog(Dialog d)
        {
            if (mDialog != null && mDialog.IsVisible)
                mDialog.Hide();
            mDialog = d;
            mDialog.Show();
        }

        protected virtual void OnMapClick(object sender, MouseButtonEventArgs e)
        {
            GeoCoordinate gc = mPage.Map.ViewportPointToLocation(e.GetPosition(mPage.Map));
            if (mState == UsageState.AddingStar)
            {
                AddStarDialog asd = new AddStarDialog(gc);
                asd.Click += new EventHandler<AddStarDialog.StarEventHandler>((o, eargs) => mPage.Dispatcher.BeginInvoke(new Action(() => { asd.Hide(); OnAddStar(eargs.Type, eargs.Position); })));
                ShowDialog(asd);
            }
            else if (mState == UsageState.AddingMapItem)
            {
                NavigateToDetailPage(gc.Longitude, gc.Latitude);
                mState = UsageState.Default;
            }
        }

        public virtual void OnAddStar(string type, GeoCoordinate position)
        {
            Star s = new Star();
            s.Type = type;
            s.X = position.Longitude;
            s.Y = position.Latitude;
            s.Note = string.Empty;           
            mConnection.Save(s, new DataAsyncCallback<Star>((e) => mPage.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (e.Error != null)
                        App.ShowToast(e.Error.Message);
                    else
                    {
                        AddStarToMap(e.DataResult);
                    }
                }))));
            mState = UsageState.Default;
        }

        private void MapItemPreview_OpenDetailClick(object sender, DataEventArgs<MapItem> e)
        {
            if (e.DataContext != null)
            {
                NavigateToDetailPage(Convert.ToString(e.DataContext.ID));
            }
            else
            {
                MessageBox.Show("Null MapItem");
            }
        }

        public virtual void OnLoadStars(List<Star> data)
        {
            if (data == null)
                data = new List<Star>();
            mStars = data;
            if (mStarsLayer != null)
                mStarsLayer.Children.Clear();
            foreach (Star s in data)
            {
                AddStarToMap(s);
            }
        }

        protected void AddStarToMap(Star s)
        {
            Image i = s.GetImage();
            i.Tag = s;
            i.MouseLeftButtonUp += new MouseButtonEventHandler((o, e) => { OnItemClick(i); });
            mStarsLayer.AddChild(i, new System.Device.Location.GeoCoordinate(s.Y, s.X));
        }

        public virtual void OnItemClick(Image source)
        {
            Star s = (Star)(source.Tag);
            if (string.IsNullOrEmpty(s.Note))
                s.Note = string.Empty;
            StarDialog scd = new StarDialog(s);
            scd.OKClick += new EventHandler<DialogEventArgs<Star>>((o, e) =>
            {
                mConnection.Save(e.DataContext, new DataAsyncCallback<Star>(eargs => mPage.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (eargs.Error != null)
                            App.ShowToast(eargs.Error.Message);
                        else
                        {                            
                            App.ShowToast(Labels.lblDone);
                        }
                    }))));
            });
            scd.DeleteClick +=  new EventHandler<DialogEventArgs<Star>>((o, e) =>
            {
                mConnection.Delete(e.DataContext, new DataAsyncCallback<Star>(eargs => mPage.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (eargs.Error != null)
                        App.ShowToast(eargs.Error.Message);
                    else
                    {
                        mStarsLayer.Children.Remove(source);
                        mStars.Remove(s);
                        App.ShowToast(Labels.lblDone);
                    }
                }))));
            });
            ShowDialog(scd);
        }

        public virtual void OnLoadMapItems(List<MapItem> data)
        {
            if (data == null)
                data = new List<MapItem>();
            mMapItems = data;
            if (mItemsLayer != null)
                mItemsLayer.Children.Clear();
            foreach (MapItem mi in data)
            {
                Image i = mi.GetImage();
                i.Tag = mi;
                i.MouseLeftButtonUp += new MouseButtonEventHandler((o, e) => { OnItemClick((MapItem)((FrameworkElement)o).Tag); });
                mItemsLayer.AddChild(mi.GetImage(), new System.Device.Location.GeoCoordinate(mi.Y, mi.X));        
            }
        }

        public virtual void OnItemClick(MapItem mi)
        {
            //MessageBox.Show(mi.Name);
            mPage.Map.Center = new System.Device.Location.GeoCoordinate(mi.Y, mi.X);
            mPage.MapItemPreview.Show(mi);
        }

        public void NavigateToDetailPage(string id)
        {
            string uri = String.Format("/View/MapItemDetail.xaml?{0}={1}", App.MAP_ITEM_ID, id);
            mPage.NavigationService.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
        }

        public void NavigateToDetailPage(double x, double y)
        {
            string uri = String.Format("/View/MapItemDetail.xaml?{0}={1}&{2}={3}",App.X, x, App.Y, y);
            mPage.NavigationService.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
        }
    }
}
