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
using Newtonsoft.Json;

namespace MyPlaces.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Detail
    {
        [JsonProperty("id")]   
        public long ID { get; set; }

        [JsonProperty("what")]   
        public string What { get; set; }

        [JsonProperty("text")]   
        public string Text { get; set; }

        [JsonProperty("time")]   
        public DateTime Time { get; set; }
    }
}
