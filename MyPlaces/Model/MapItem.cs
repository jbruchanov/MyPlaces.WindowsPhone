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
    [JsonObject(MemberSerialization.OptOut)]
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
        
        [JsonProperty("city")]
        public string City { get; set; }
        
        [JsonProperty("street")]
        public string Street { get; set; }
        
        [JsonProperty("web")]
        public string Web { get; set; }
        
        [JsonProperty("streetViewLink")]
        public string StreetViewLink { get; set; }
        
        [JsonProperty("author")]
        public string Author { get; set; }
        
        [JsonProperty("contact")]
        public string Contact { get; set; }
        
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
