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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MyPlaces.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MapItem : INotifyPropertyChanged
    {
        private long mID;
        [JsonProperty("id")]
        public long ID { get { return mID; } set { mID = value; NotifyPropertyChange("ID"); } }

        private string mType;
        [JsonProperty("type")]
        public string Type { get { return mType; } set { mType = value; mImage = null; NotifyPropertyChange("Type"); } }

        private string mName;
        [JsonProperty("name")]
        public string Name { get { return mName; } set { mName = value; NotifyPropertyChange("Name"); } }

        private string mCountry;
        [JsonProperty("country")]
        public string Country { get { return mCountry; } set { mCountry = value; NotifyPropertyChange("Country"); } }

        private string mCity;
        [JsonProperty("city")]
        public string City { get { return mCity; } set { mCity = value; NotifyPropertyChange("City"); } }

        private string mStreet;
        [JsonProperty("street")]
        public string Street { get { return mStreet; } set { mStreet = value; NotifyPropertyChange("Street"); } }

        private string mWeb;
        [JsonProperty("web")]
        public string Web { get { return mWeb; } set { mWeb = value; NotifyPropertyChange("Web"); } }

        private string mStreetViewLink;
        [JsonProperty("streetViewLink")]
        public string StreetViewLink { get { return mStreetViewLink; } set { mStreetViewLink = value; NotifyPropertyChange("StreetViewLink"); } }

        private string mAuthor;
        [JsonProperty("author")]
        public string Author { get { return mAuthor; } set { mAuthor = value; NotifyPropertyChange("Author"); } }

        private string mContact;
        [JsonProperty("contact")]
        public string Contact { get { return mContact; } set { mContact = value; NotifyPropertyChange("Contact"); } }

        private double mX;
        [JsonProperty("x")]
        public double X { get { return mX; } set { mX = value; NotifyPropertyChange("X"); } }

        private double mY;
        [JsonProperty("y")]
        public double Y { get { return mY; } set { mY = value; NotifyPropertyChange("Y"); } }

        private int mRating;
        [JsonProperty("rating")]
        public int Rating { get { return mRating; } set { mRating = value; NotifyPropertyChange("Rating"); } }

        private ObservableCollection<string> mPros;
        [JsonProperty("pros")]
        public ICollection<string> Pros { get { return mPros; } set { mPros = new ObservableCollection<string>(value); NotifyPropertyChange("Pros"); } }

        private ObservableCollection<string> mCons;
        [JsonProperty("cons")]
        public ICollection<string> Cons { get { return mCons; } set { mCons = new ObservableCollection<string>(value); NotifyPropertyChange("Cons"); } }

        private ObservableCollection<Detail> mDetails;
        [JsonProperty("details")]
        public ICollection<Detail> Details { get { return mDetails; } set { mDetails = new ObservableCollection<Detail>(value); NotifyPropertyChange("Details"); } }

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
            else if (t.Equals("sushi"))
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_sushi.png", UriKind.RelativeOrAbsolute)) };
            else
                result = new Image { Source = new BitmapImage(new Uri("/Resources/Images/ico_search.png", UriKind.RelativeOrAbsolute)) };


            result.Stretch = System.Windows.Media.Stretch.None;
            mImage = result;
            return mImage;
        }

        private void NotifyPropertyChange(string Name)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(Name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
