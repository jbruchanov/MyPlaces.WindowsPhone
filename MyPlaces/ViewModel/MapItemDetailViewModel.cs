﻿using System;
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

        public MapItemDetailViewModel(MapItemDetail page)
        {
            mPage = page;
            mPage.Loaded += (o, e) => { InitAppBar(); Init(); };
            
            
        }

        private void Init()
        {
            mServerConnection = new ServerConnection();
            mMapItemId = mPage.NavigationContext.QueryString[App.MAP_ITEM_ID];
            if(String.IsNullOrEmpty(mMapItemId))
            {
                mPage.NavigationService.GoBack();
            }
            else
            {
                mServerConnection.GetMapItemTypes(new DataAsyncCallback<List<string>>((res) => { mPage.Dispatcher.BeginInvoke(() => OnDownloadMapItemTypes(res.DataResult)); }));
            }

            mPage.RootPivot.SelectionChanged += new SelectionChangedEventHandler(RootPivot_SelectionChanged);
        }

        public virtual void OnDownloadMapItemTypes(List<string> list)
        {
            mPage.lpType.ItemsSource = list;
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
            mPage.DataContext = mi;
        }

        public MapItem GetMapItem()
        {
            return null;
        }
    }
}