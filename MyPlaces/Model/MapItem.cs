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
using System.Collections.Generic;

namespace MyPlaces.Model
{
    public class MapItem
    {
        private long id { get; set; }
        private string type { get; set; }

        private string name { get; set; }
        private string country { get; set; }
        private string city { get; set; }
        private string street { get; set; }
        private string web { get; set; }
        private string streetViewLink { get; set; }
        private string author { get; set; }
        private string contact { get; set; }

        private double x { get; set; }
        private double y { get; set; }

        private int rating { get; set; }

        private List<string> pros { get; set; }
        private List<string> cons { get; set; }
        private List<Detail> details { get; set; }
    }
}
