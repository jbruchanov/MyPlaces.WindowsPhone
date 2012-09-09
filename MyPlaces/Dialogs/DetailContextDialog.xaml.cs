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
using MyPlaces.Resources;

namespace MyPlaces.Dialogs
{
    public partial class DetailContextDialog : ModalDialog, HasOkButton<DetailContextDialog.DetailContextTypeEventArgs>
    {
        public event EventHandler<DetailContextTypeEventArgs> OKClick;
        private Detail mDetail;
        public bool AllowEmptyValues { get; set; }

        public DetailContextDialog(Detail d = null)
        {
            InitializeComponent();
            OK.Click += new RoutedEventHandler(OnOKClick);
            Cancel.Click += (o, e) => Hide();
            
            if (d == null)
                d = new Detail { Text = string.Empty, What = string.Empty, Time = DateTime.Now };
            SetDetail(d);
            AllowEmptyValues = false;
        }

        protected virtual void OnOKClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Title.Text) && !AllowEmptyValues)
                    throw new Exception(Labels.lblEmptyValues);

                if (OKClick != null)
                {
                    OKClick.Invoke(this, new DetailContextTypeEventArgs(GetDetail()));
                }
                Hide();
            }
            catch (Exception ex)
            {
                App.ShowMessage(ex.Message);
            }
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
