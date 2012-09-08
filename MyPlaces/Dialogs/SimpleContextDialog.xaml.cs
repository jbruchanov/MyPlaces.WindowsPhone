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

namespace MyPlaces.Dialogs
{
    public partial class SimpleContextDialog : ModalDialog, HasOkButton<SimpleContextDialog.SimpleContextTypeEventArgs>
    {
        public event EventHandler<SimpleContextTypeEventArgs> OKClick;
        private ContextItemType mType;

        public SimpleContextDialog(ContextItemType type = ContextItemType.Pro, string value = null)
        {
            InitializeComponent();
            if (type == ContextItemType.Con)
            {
                TypeIcon.Source = new BitmapImage(new Uri("/Resources/Images/ico_minus.png", UriKind.RelativeOrAbsolute));
            }
            mType = type;
            
            if (value != null)
                Value.Text = value;

            OK.Click += new RoutedEventHandler(OnOKClick);
            Cancel.Click += (o, e) => Hide();
        }

        protected virtual void OnOKClick(object sender, RoutedEventArgs e)
        {
            if (OKClick != null)
                OKClick.Invoke(this, new SimpleContextTypeEventArgs(Value.Text, mType));
            Hide();
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

        event EventHandler<SimpleContextTypeEventArgs> HasOkButton<SimpleContextTypeEventArgs>.OKClick
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }
    }
}
