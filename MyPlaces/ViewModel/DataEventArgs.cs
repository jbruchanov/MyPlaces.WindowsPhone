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

namespace MyPlaces.ViewModel
{
    public class DataEventArgs<T> : EventArgs
    {
        public T DataContext { get; private set; }

        public DataEventArgs(T item)
        {
            DataContext = item;
        }
    }
}
