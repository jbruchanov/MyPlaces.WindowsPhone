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
using System.Windows.Controls.Primitives;

namespace MyPlaces.Dialogs
{
    public class Dialog : UserControl
    {
        private Popup mParent;
        public bool IsVisible
        {
            get
            {
                return mParent != null && mParent.IsOpen;
            }
        }
        public class DialogEventArgs : EventArgs
        {
            public enum ButtonType
            {
                Add, Cancel, Remove, OK
            }

            public ButtonType Type { get; set; }
            public RoutedEventArgs SourceEvent { get; set; }
            public object SourceSender { get; set; }
        }

        public virtual void Show()
        {
            if (mParent == null)
            {
                mParent = new Popup();
                mParent.Child = this;
            }
            mParent.IsOpen = true;
        }

        public virtual bool Hide()
        {
            bool result = false;
            if (mParent != null)
            {
                result = mParent.IsOpen;
                mParent.IsOpen = false;
            }
            return result;
        }

        protected Popup GetPopup()
        {
            return mParent;
        }
    }
}
