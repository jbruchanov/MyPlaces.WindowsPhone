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
using Newtonsoft.Json;

namespace MyPlaces.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MapItem
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("City")]
        public string city { get; set; }
        [JsonProperty("Street")]
        public string street { get; set; }
        [JsonProperty("Web")]
        public string web { get; set; }
        [JsonProperty("StreetViewLink")]
        public string StreetViewLink { get; set; }
        [JsonProperty("Author")]
        public string author { get; set; }
        [JsonProperty("Contact")]
        public string contact { get; set; }
        [JsonProperty("x")]
        public double X { get; set; }
        [JsonProperty("y")]
        public double Y { get; set; }
        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("pros")]
        public List<string> Pros { get; set; }
        [JsonProperty("cons")]
        public List<string> Cons { get; set; }
        [JsonProperty("details")]
        public List<Detail> Details { get; set; }
    }
}
