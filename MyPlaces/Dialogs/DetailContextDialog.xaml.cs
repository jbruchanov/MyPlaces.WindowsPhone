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
using MyPlaces.Model;

namespace MyPlaces.Dialogs
{
    public partial class DetailContextDialog : ModalDialog
    {
        public event EventHandler<DetailContextTypeEventArgs> OKClick;
        private Detail mDetail;

        public DetailContextDialog(Detail d = null)
        {
            OK.Click += new RoutedEventHandler(OnOKClick);
            Cancel.Click += (o, e) => Hide();
            
            if (d == null)
                d = new Detail { Text = string.Empty, What = string.Empty, Time = DateTime.Now };
            SetDetail(d);
        }

        protected virtual void OnOKClick(object sender, RoutedEventArgs e)
        {
            if (OKClick != null)
            {
                OKClick.Invoke(this, new DetailContextTypeEventArgs(GetDetail()));
            }
            Hide();
        }

        public class DetailContextTypeEventArgs : EventArgs
        {
            public Detail Value { get; private set; }
            public DetailContextTypeEventArgs(Detail d)
            {
                Value = d;
            }
        }

        public void SetDetail(Detail d)
        {
            mDetail = d;
            Title.Text = d.What;
            Detail.Text = d.Text;
            Date.Text = d.Time.ToLongDateString();
        }

        public Detail GetDetail()
        {
            mDetail.Text = Title.Text;
            mDetail.What = Detail.Text;
            return mDetail;
        }
    }
}
