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
    public partial class AddNewContextItemDialog : ModalDialog
    {

        public event EventHandler<AddNewContextItemDialogEventArgs> Click;
        public AddNewContextItemDialog()
        {
            InitializeComponent();
            Plus.Click += new RoutedEventHandler(Star_Click);
            Minus.Click += new RoutedEventHandler(Minus_Click);
            Detail.Click += new RoutedEventHandler(Detail_Click);
        }

        void Detail_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click.Invoke(this, new AddNewContextItemDialogEventArgs(ContextItemType.Detail));
            Hide();
        }

        void Minus_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click.Invoke(this, new AddNewContextItemDialogEventArgs(ContextItemType.Con));
            Hide();
        }

        void Star_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click.Invoke(this, new AddNewContextItemDialogEventArgs(ContextItemType.Pro));
            Hide();
        }

        public class AddNewContextItemDialogEventArgs : EventArgs
        {
            public ContextItemType Type { get; private set; }

            public AddNewContextItemDialogEventArgs(ContextItemType type)
            {
                Type = type;
            }
        }
    }
}
