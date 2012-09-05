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
using System.ComponentModel;

namespace MyPlaces.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Detail : INotifyPropertyChanged
    {
        private long mID;
        [JsonProperty("id")]
        public long ID { get { return mID; } set { mID = value; NotifyPropertyChange("ID"); } }

        private string mWhat;
        [JsonProperty("what")]
        public string What { get { return mWhat; } set { mWhat = value; NotifyPropertyChange("What"); } }

        private string mText;
        [JsonProperty("text")]
        public string Text { get { return mText; } set { mText = value; NotifyPropertyChange("Text"); } }

        private DateTime mTime;
        [JsonProperty("time")]
        public DateTime Time { get { return mTime; } set { mTime = value; NotifyPropertyChange("Time"); } }


        private void NotifyPropertyChange(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
