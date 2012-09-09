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
using System.Windows.Media.Imaging;
using MyPlaces.Resources;

namespace MyPlaces.Dialogs
{
    public partial class SimpleContextDialog : ModalDialog, HasOkButton<SimpleContextDialog.SimpleContextTypeEventArgs>
    {
        public event EventHandler<SimpleContextDialog.SimpleContextTypeEventArgs> OKClick;
        private ContextItemType mType;
        public bool AllowEmptyValues { get; set; }

        public SimpleContextDialog(ContextItemType type = ContextItemType.Pro, string value = null)
        {
            InitializeComponent();
            if (type == ContextItemType.Con)
            {
                IconImageSource = new BitmapImage(new Uri("/Resources/Images/ico_minus.png", UriKind.RelativeOrAbsolute));
            }
            mType = type;
            
            if (value != null)
                Value.Text = value;

            OK.Click += new RoutedEventHandler(OnOKClick);
            Cancel.Click += (o, e) => Hide();
        }

        public SimpleContextDialog(string value, ImageSource icon = null)
        {
            InitializeComponent();
            Value.Text = value;
            if (icon != null)
                IconImageSource = icon;
            OK.Click += new RoutedEventHandler(OnOKClick);
            Cancel.Click += (o, e) => Hide();
        }

        public ImageSource IconImageSource
        {
            get
            {
                return TypeIcon.Source;
            }
            set
            {
                TypeIcon.Source = value;
            }
        }

        protected virtual void OnOKClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Value.Text) && !AllowEmptyValues)
                    throw new Exception(Labels.lblEmptyValues);

                if (OKClick != null)
                    OKClick.Invoke(this, new SimpleContextTypeEventArgs(Value.Text, mType));
                Hide();
            }
            catch (Exception ex)
            {
                App.ShowMessage(ex.Message);
            }
        }

        public class SimpleContextTypeEventArgs : EventArgs
        {
            public string Value { get; private set; }
            public ContextItemType Type { get; private set; }
            public SimpleContextTypeEventArgs(string value, ContextItemType type)
            {
                Value = value;
                Type = type;
            }
        }
    }
}
