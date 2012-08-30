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
using System.Windows.Media.Imaging;

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

        private Image mImage;

        public Image GetImage()
        {
            if (mImage != null)
                return mImage;

            Image result = null;
            string t = Type;
            if (String.IsNullOrEmpty(t))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_pin.png", UriKind.RelativeOrAbsolute)) };
            else
                t = t.ToLower();

            if (t.Equals("hospoda"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_beer.png", UriKind.RelativeOrAbsolute)) };
            else if (t.Equals("bar"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_drink.png", UriKind.RelativeOrAbsolute)) };
            else if (t.Equals("kavárna"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_cafe.png", UriKind.RelativeOrAbsolute)) };
            else if (t.Equals("café"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_cafe.png", UriKind.RelativeOrAbsolute)) };
            else if (t.Equals("restaurace"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_restaurant.png", UriKind.RelativeOrAbsolute)) };
            else if (t.Equals("pizzerie"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_pizza.png", UriKind.RelativeOrAbsolute)) };
            else if (t.Equals("fastfood"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_fastfood.png", UriKind.RelativeOrAbsolute)) };
            else if (t.Equals("club"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_music.png", UriKind.RelativeOrAbsolute)) };
            else if (t.Equals("zahrádka"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_patio.png", UriKind.RelativeOrAbsolute)) };
            else
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_search.png", UriKind.RelativeOrAbsolute)) };


            result.Stretch = System.Windows.Media.Stretch.None;
            mImage = result;
            return mImage;
        }
    }
}
