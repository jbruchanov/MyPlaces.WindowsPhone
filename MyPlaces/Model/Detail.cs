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
    public class Detail
    {
        public long Id { get; set; }
        public string What { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
