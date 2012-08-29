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
    public class Star
    {
        [JsonProperty("id")]      
        public long ID {get;set;}
        [JsonProperty("note")]  
        public string Note {get;set;}
        [JsonProperty("type")]  
        public string Type {get;set;}
        [JsonProperty("x")]  
        public double X {get;set;}
        [JsonProperty("y")]  
        public double Y {get;set;}
    }
}
