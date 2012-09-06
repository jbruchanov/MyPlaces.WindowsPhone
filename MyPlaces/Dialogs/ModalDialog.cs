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

namespace MyPlaces.Dialogs
{
    public class ModalDialog : Dialog
    {
        public ModalDialog():base()
        {
            this.Width = Application.Current.RootVisual.RenderSize.Width;
            this.Height = Application.Current.RootVisual.RenderSize.Height;
        }
    }
}
