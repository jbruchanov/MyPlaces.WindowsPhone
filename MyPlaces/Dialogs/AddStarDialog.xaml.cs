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
using MyPlaces.Utils;
using System.Device.Location;

namespace MyPlaces.Dialogs
{
    public partial class AddStarDialog : ModalDialog
    {
        public event EventHandler<StarEventHandler> Click;
        private GeoCoordinate mPosition;
        private double mY;

        public AddStarDialog(GeoCoordinate position)
        {
            InitializeComponent();
            mPosition = position;
            Init();
        }

        private void Init()
        {
            foreach (Button b in ControlUtils.FindVisualChildren<Button>(this))
            {
                b.Click += new RoutedEventHandler(OnButtonClick);
            }
        }

        protected virtual void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (Click != null)
                Click.Invoke(this, new StarEventHandler(b.Tag as string, mPosition));
        }


        public class StarEventHandler : EventArgs
        {
            public string Type { get; private set; }
            public GeoCoordinate Position { get; private set; }

            public StarEventHandler(string type, GeoCoordinate position)
            {
                Type = type;
                Position = position;
            }
        }
    }
}
