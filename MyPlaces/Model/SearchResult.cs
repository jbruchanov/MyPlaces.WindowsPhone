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

namespace MyPlaces.Model
{
    public class SearchResult
    {
        public enum Type
        {
            MapItem,
            Detail,
            Pros,
            Cons
        }

        public Type type { get; set; }
        public Object searchedItem { get; set; }
    }
}
