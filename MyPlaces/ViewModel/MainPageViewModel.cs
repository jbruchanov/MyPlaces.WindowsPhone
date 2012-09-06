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

namespace MyPlaces.ViewModel
{
    public class MainPageViewModel
    {
        private MainPage mPage;
        private List<Star> mStars;
        private List<MapItem> mMapItems;
        private ServerConnection mConnection;
        private MapLayer mSmileysLayer;
        private MapLayer mItemsLayer;

        public MainPageViewModel(MainPage page)
        {
            mPage = page;
            mPage.Loaded += new RoutedEventHandler((o,e) => Init());
            mPage.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(OnBackPress);
        }

        void OnBackPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mAddMapItemDialog != null)
            {
                e.Cancel = mAddMapItemDialog.Hide();
                mAddMapItemDialog = null;
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
            mSmileysLayer = new MapLayer();
            mItemsLayer = new MapLayer();

            mPage.Map.Children.Add(mSmileysLayer);
            mPage.Map.Children.Add(mItemsLayer);
            mConnection = new ServerConnection();
            mConnection.GetStars(new DataAsyncCallback<List<Star>>((res) => { mPage.Dispatcher.BeginInvoke(new Action(() => OnLoadStars(res.DataResult))); }));
            mConnection.GetMapItems(new DataAsyncCallback<List<MapItem>>((res) => { mPage.Dispatcher.BeginInvoke(new Action(() => OnLoadMapItems(res.DataResult))); }));
            mPage.MapItemPreview.OpenDetailClick += new EventHandler<DataEventArgs<MapItem>>(MapItemPreview_OpenDetailClick);
            mPage.Map.MouseLeftButtonUp += new MouseButtonEventHandler(OnMapClick);
            mPage.AddButton.Click += new RoutedEventHandler(OnAddClick);
        }

        private AddNewItemDialog mAddMapItemDialog;
        void OnAddClick(object sender, RoutedEventArgs e)
        {
            AddStarDialog asd = new AddStarDialog();
            asd.Show();

            //if(mAddMapItemDialog == null)
            //    mAddMapItemDialog = new AddNewItemDialog(sender as FrameworkElement);
            //mAddMapItemDialog.Show();
        }

        void OnMapClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void MapItemPreview_OpenDetailClick(object sender, DataEventArgs<MapItem> e)
        {
            if (e.DataContext != null)
            {
                string uri = String.Format("/View/MapItemDetail.xaml?{0}={1}", App.MAP_ITEM_ID, e.DataContext.ID);
                mPage.NavigationService.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute));
            }
            else
            {
                MessageBox.Show("Null MapItem");
            }
        }

        public virtual void OnLoadStars(List<Star> data)
        {
            mStars = data;
            foreach (Star s in data)
            {
                Image i = s.GetImage();
                i.Tag = s;
                i.MouseLeftButtonUp += new MouseButtonEventHandler((o, e) => { OnItemClick((Star)((FrameworkElement)o).Tag); });
                mSmileysLayer.AddChild(i, new System.Device.Location.GeoCoordinate(s.Y, s.X));
                
            }
        }

        public virtual void OnItemClick(Star s)
        {
            MessageBox.Show(s.Note);
            //mPage.MapItemPreview.Show();
        }

        public virtual void OnLoadMapItems(List<MapItem> data)
        {
            mMapItems = data;
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
    }
}
