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
using MyPlaces.Dialogs;
using Coding4Fun.Phone.Controls;
using MyPlaces.Resources;
using System.Threading;
using MyPlaces.GeocodeService;
using System.Collections.ObjectModel;

namespace MyPlaces.ViewModel
{
    public class MapItemDetailViewModel
    {
        private const string SEARCH_ICON = "/Resources/Icons/AppBarWhite/appbar.feature.search.rest.png";
        private const string EDIT_ICON = "/Resources/Icons/AppBarWhite/appbar.edit.rest.png";

        private MapItemDetail mPage;
        private ServerConnection mServerConnection;
        private string mMapItemId;
        public MapItem MapItem { get; private set; }
        public ICollection<FrameworkElement> ContextItems { get; set; }

        private ApplicationBarIconButton mDeleteButton;
        private ApplicationBarIconButton mSaveButton;
        private ApplicationBarIconButton mSearchEditButton;
        private ApplicationBarIconButton mAddButton;
        private Dialog mDialog;

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
            mPage.lbContext.SelectionChanged += new SelectionChangedEventHandler(OnSelectionContextChange);
        }

        protected virtual void OnSelectionContextChange(object sender, SelectionChangedEventArgs e)
        {
            bool b = e.AddedItems.Count > 0;
            mSearchEditButton.IsEnabled = b;
            mDeleteButton.IsEnabled = b;
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
                MapItem.X = coord.Longitude;
                MapItem.Y = coord.Latitude;
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
            mPage.lbContext.SelectedIndex = -1;
            if (mPage.RootPivot.SelectedIndex == 2)
            {
                mSearchEditButton.IconUri = new Uri(EDIT_ICON, UriKind.RelativeOrAbsolute);
                mSearchEditButton.IsEnabled = false;
            }
            else
            {
                mSearchEditButton.IconUri = new Uri(SEARCH_ICON, UriKind.RelativeOrAbsolute);
                mSearchEditButton.IsEnabled = true;
            }
            mDeleteButton.IsEnabled = mPage.RootPivot.SelectedIndex == 0;
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

            mSearchEditButton = new ApplicationBarIconButton
            {
                IconUri = new Uri(SEARCH_ICON, UriKind.RelativeOrAbsolute),
                Text = Resources.Labels.lblSearch
            };
            mSearchEditButton.Click += new EventHandler(OnApplicationBarButtonClick);
            mPage.ApplicationBar.Buttons.Add(mSearchEditButton);

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
            else if (mSearchEditButton == sender)
            {
                if (mPage.RootPivot.SelectedIndex == 2)
                    OnEditContextClick();
                else
                    OnSearchClick();
            }
            else if (mSaveButton == sender)
                OnSaveClick();
        }

        public virtual void OnSaveClick()
        {

        }

        public virtual void OnEditContextClick()
        {

        }
        public virtual void OnSearchClick()
        {
            if (mPushpin != null)
            {
                if (mPage.RootPivot.SelectedIndex != 0)
                    mPage.RootPivot.SelectedIndex = 0;
                OnLocationSearch(mPushpin.Location.Longitude, mPushpin.Location.Latitude);
            }
            else
            {
                ToastPrompt tp = new ToastPrompt();
                tp.Message = Labels.errSelectPlaceOnMap;
                tp.Show();
            }
        }

        protected virtual void OnLocationSearch(double x, double y)
        {
            try
            {
                mSearchEditButton.IsEnabled = false;
                GeocodeServiceClient c = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                ReverseGeocodeRequest r = new ReverseGeocodeRequest();
                r.Location = new Microsoft.Phone.Controls.Maps.Platform.Location() { Longitude = x, Latitude = y };
                r.Credentials = new Credentials();
                r.Credentials.ApplicationId = "AvSVwNylWZFgFYUZ7OathJMmY1L2Gd4bObRisS0vNvrAObiscGbb7n45oMgXi5bv";
                c.ReverseGeocodeCompleted += new EventHandler<ReverseGeocodeCompletedEventArgs>(OnReverseGeocode);
                c.ReverseGeocodeAsync(r);
            }
            catch (Exception e)
            {
                ShowToast(e.Message);
                mSearchEditButton.IsEnabled = true;
            }
        }

        protected virtual void OnReverseGeocode(object sender, ReverseGeocodeCompletedEventArgs e)
        {
            mSearchEditButton.IsEnabled = true;
            if (e.Error != null)
            {
                ShowToast(e.Error.Message);
                return;
            }

            if (e.Result.ResponseSummary.StatusCode == ResponseStatusCode.Success)
            {
                ObservableCollection<GeocodeResult> results = e.Result.Results;
                if (results.Count == 1)
                {
                    GeocodeResult res = results[0];
                    string street = res.Address.AddressLine;
                    string city = res.Address.Locality;
                    string country = res.Address.CountryRegion;

                    mPage.txtStreet.Text = street;
                    mPage.txtCity.Text = city;
                    mPage.txtCountry.Text = country;
                    ShowToast(res.Address.FormattedAddress, Labels.lblFound);
                }
                else
                {
                    ShowToast("Too many resultst //TODO!");
                }
            }
            else
                ShowToast(e.Result.ResponseSummary.FaultReason);
        }

        private void ShowToast(string msg, string title = "")
        {
            ToastPrompt tp = new ToastPrompt();
            tp.Title = title;
            tp.Message = msg;
            tp.TextWrapping = TextWrapping.Wrap;
            tp.Show();
        }

        public virtual void OnDeleteClick()
        {

        }

        public virtual void OnAddClick()
        {
            AddNewContextItemDialog adci = new AddNewContextItemDialog();
            adci.Click += (o,e) =>
                {
                    if (e.Type == AddNewContextItemDialog.ContextItemType.Pro)
                    {
                        SimpleContextDialog scd = new SimpleContextDialog();
                        scd.OKClick += (src, eargs) =>
                            {
                                ShowToast(eargs.Value);
                            };
                        ShowDialog(scd);
                    }
                    else if (e.Type == AddNewContextItemDialog.ContextItemType.Con)
                    {
                        SimpleContextDialog scd = new SimpleContextDialog(SimpleContextDialog.SimpleContextType.Con);
                        scd.OKClick += (src, eargs) =>
                        {
                            ShowToast(eargs.Value);
                        };
                        ShowDialog(scd);
                    }
                    else
                        ShowToast("//TODO");
                };
            ShowDialog(adci);
        }

        public void ShowDialog(Dialog d)
        {
            if (mDialog != null && mDialog.IsVisible)
                mDialog.Hide();
            mDialog = d;
            mDialog.Show();
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
            MapItem = mi;
            ContextItems = CreateList(mi);
            mPage.RootPivot.Title = mi.Name;
            mPage.DataContext = this;
            if (mPushpin == null)
            {
                mPushpin = new Pushpin { Location = new GeoCoordinate { Longitude = mi.X, Latitude = mi.Y } };
                mPage.Map.Children.Add(mPushpin);
            }
            else
                mPushpin.Location = new GeoCoordinate { Longitude = mi.X, Latitude = mi.Y };
        }

        private List<FrameworkElement> CreateList(MapItem mi)
        {
            List<FrameworkElement> result = new List<FrameworkElement>();
            if (mi.Pros != null)
            {
                foreach (string pro in mi.Pros)
                    result.Add(new MapItemContextView(pro, MapItemContextView.ContextType.Pro));
            }
            if (mi.Cons != null)
            {
                foreach (string con in mi.Cons)
                    result.Add(new MapItemContextView(con, MapItemContextView.ContextType.Con));
            }
            if (mi.Details != null)
            {
                foreach (Detail d in mi.Details)
                    result.Add(new MapItemContextView(d, MapItemContextView.ContextType.Detail));
            }
            return result;
        }

        public MapItem GetMapItem()
        {
            return null;
        }
    }
}
