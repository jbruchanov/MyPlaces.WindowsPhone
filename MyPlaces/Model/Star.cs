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
using System.Windows.Media.Imaging;

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

        public Image GetImage()
        {
            Image result = null;
            if (Type.Equals("10"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_star.png", UriKind.RelativeOrAbsolute)) };
            else if (Type.Equals("11"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_cafe.png", UriKind.RelativeOrAbsolute)) };
            else if (Type.Equals("12"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_drink.png", UriKind.RelativeOrAbsolute)) };
            else if (Type.Equals("13"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_wine.png", UriKind.RelativeOrAbsolute)) };
            else if (Type.Equals("14"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_search.png", UriKind.RelativeOrAbsolute)) };
            else if (Type.Equals("20"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_smile_happy.png", UriKind.RelativeOrAbsolute)) };
            else if (Type.Equals("21"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_smile_lick.png", UriKind.RelativeOrAbsolute)) };
            else if (Type.Equals("22"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_smile_neutral.png", UriKind.RelativeOrAbsolute)) };
            else if (Type.Equals("23"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_smile_unhappy.png", UriKind.RelativeOrAbsolute)) };
            else if (Type.Equals("24"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_smile_veryhappy.png", UriKind.RelativeOrAbsolute)) };

            //result.Opacity = 0.8;
            result.Stretch = System.Windows.Media.Stretch.None;
            return result;
        }
    }
}
