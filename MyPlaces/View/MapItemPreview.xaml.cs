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

namespace MyPlaces.View
{
    public partial class MapItemPreview : UserControl
    {
        private bool mIsVisible = true;
        public MapItemPreview()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (mIsVisible)
                HideAnim.Begin();
        }


        public void Show()
        {
            if (!mIsVisible)
                ShowAnim.Begin();
        }

        public void Hide()
        {
            if (mIsVisible)
                HideAnim.Begin();
        }

        private void HideAnim_Completed(object sender, EventArgs e)
        {
            mIsVisible = false;
        }

        private void ShowAnim_Completed(object sender, EventArgs e)
        {
            mIsVisible = true;
        }
    }
}
