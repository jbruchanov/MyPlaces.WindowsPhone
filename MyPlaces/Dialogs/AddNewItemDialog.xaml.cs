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
using System.Windows.Controls.Primitives;

namespace MyPlaces.Dialogs
{
    public partial class AddNewItemDialog : Dialog
    {
        public event EventHandler<RoutedEventArgs> Click;
        private FrameworkElement mAlignTo;
        public AddNewItemDialog()
        {
            InitializeComponent();
            Star.Click += new RoutedEventHandler(Star_Click);
            MapItem.Click += new RoutedEventHandler(MapItem_Click);
        }

        public AddNewItemDialog(FrameworkElement b)
            : this()
        {
            mAlignTo = b;
        }

        void MapItem_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click.Invoke(sender, e);
            Hide();
        }

        void Star_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click.Invoke(sender, e);
            Hide();
        }

        public override void Show()
        {
            base.Show();
            if(mAlignTo != null)
            {
                Popup p = GetPopup();
                var transform = mAlignTo.TransformToVisual(Application.Current.RootVisual);
                Point offset = transform.Transform(new Point(0,0));
                p.VerticalOffset = offset.Y + mAlignTo.RenderSize.Height + 1;
                p.HorizontalOffset = System.Windows.Application.Current.Host.Content.ActualWidth - this.MinWidth - 1;
            }
        }
    }
}
