﻿using System;
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
using Microsoft.Phone.Tasks;
using MyPlaces.ViewModel;
using Coding4Fun.Phone.Controls;

namespace MyPlaces.View
{
    public partial class MapItemPreview : UserControl
    {
        public bool IsVisible { get; private set; }
        private MapItem mItem;
        public event EventHandler<DataEventArgs<MapItem>> OpenDetailClick;

        public MapItemPreview()
        {
            InitializeComponent();
            IsVisible = false;
            LayoutRoot.Height = 0;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
                HideAnim.Begin();
        }


        public void Show(MapItem mi = null)
        {
            if (mi != null)
                SetMapItem(mi);
            if (!IsVisible)
                ShowAnim.Begin();
        }

        public void Hide()
        {
            if (IsVisible)
                HideAnim.Begin();
        }

        private void HideAnim_Completed(object sender, EventArgs e)
        {
            IsVisible = false;
        }

        private void ShowAnim_Completed(object sender, EventArgs e)
        {
            IsVisible = true;
        }

        public void SetMapItem(MapItem mi)
        {
            mItem = mi;
            PlaceName.Text = mi.Name;
            PlaceLocation.Text = String.Format("{0}, {1}", mi.Street, mi.City);
            StreetView.IsEnabled = !String.IsNullOrEmpty(mi.StreetViewLink);
            Phone.IsEnabled = !String.IsNullOrEmpty(mi.Contact);
            Web.IsEnabled = !String.IsNullOrEmpty(mi.Web);
            Icon.Source = mi.GetImage().Source;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Web == sender)
                OnWebButtonClick();
            else if (StreetView == sender)
                OnStreetViewClick();
            else if (Phone == sender)
                OnPhoneClick();
            else if (Share == sender)
                OnShareClick();
            else if (More == sender)
                OnMoreClick();

        }

        public virtual void OnMoreClick()
        {
            if (OpenDetailClick != null)
            {
                OpenDetailClick.Invoke(this, new DataEventArgs<MapItem>(mItem));
                Hide();
            }
        }

        public virtual void OnShareClick()
        {
            
        }

        public virtual void OnPhoneClick()
        {
            PhoneCallTask pct = new PhoneCallTask();
            pct.DisplayName = mItem.Name;
            pct.PhoneNumber = mItem.Contact;
            pct.Show();
        }

        public virtual void OnStreetViewClick()
        {
            
        }

        public virtual void OnWebButtonClick()
        {
            try
            {
                WebBrowserTask task = new WebBrowserTask();
                string url = mItem.Web;
                if (!url.StartsWith(App.HTTP_PREFIX))
                    url = App.HTTP_PREFIX + url;
                task.Uri = new Uri(url);
                task.Show();
                Hide();
            }
            catch (Exception e)
            {
                ToastPrompt tp = new ToastPrompt();
                tp.Message = e.Message;
                tp.Show();
            }
        }
    }
}
