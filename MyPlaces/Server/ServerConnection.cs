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
using MyPlaces.Model;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MyPlaces.Server
{
    public class ServerConnection
    {
        private string mServerUrl;
        private string mStarsUrl;
        private string mMapItemsUrl;

        private const string STARS_SUFFIX = "/stars";
        private const string MAPITEMS_COORDS_SUFFIX = "/mapitems/{0}/{1}/{2}/{3}";

        public ServerConnection(string url = "http://myplaces.scurab.com:8182")
        {
            mServerUrl = url;
            mStarsUrl = mServerUrl + STARS_SUFFIX;
            mMapItemsUrl = mServerUrl + MAPITEMS_COORDS_SUFFIX;
        }

        /// <summary>
        /// Downloads stars from server
        /// </summary>
        /// <param name="Callback"></param>
        public void GetStars(DataAsyncCallback<List<Star>> Callback)
        {
            WebRequest req = WebRequest.CreateHttp(mStarsUrl);
            req.Method = "GET";
            req.BeginGetResponse(new AsyncCallback(
                (IAsyncResult result) =>
                {
                    WebResponse s = req.EndGetResponse(result);
                    string json = ReadStreamToEnd(s.GetResponseStream());
                    s.Close();
                    
                    List<Star> dataResult = JsonConvert.DeserializeObject<List<Star>>(json);
                    Callback(new DataAsyncResult<List<Star>>(dataResult));                  
                }), req);            
        }

      
        private string ReadStreamToEnd(Stream s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buffer = new byte[4096];
            int n = 0;
            while ((n = s.Read(buffer, 0, buffer.Length)) > 0)
            {
                string sub = System.Text.Encoding.UTF8.GetString(buffer, 0, n);
                sb.Append(sub);
            }
            return sb.ToString();
        }

        public void GetMapItems(DataAsyncCallback<List<MapItem>> dataAsyncCallback)
        {
            string url = String.Format(mMapItemsUrl,0,0,90,90);
            WebRequest req = WebRequest.CreateHttp(url);
            req.Method = "GET";
            req.BeginGetResponse(new AsyncCallback(
                (IAsyncResult result) =>
                {
                    WebResponse s = req.EndGetResponse(result);
                    string json = ReadStreamToEnd(s.GetResponseStream());
                    s.Close();

                    List<MapItem> dataResult = JsonConvert.DeserializeObject<List<MapItem>>(json);
                    dataAsyncCallback(new DataAsyncResult<List<MapItem>>(dataResult));
                }), req);     
        }
    }

    

    public class DataAsyncResult<T> : IAsyncResult
    {
        public T DataResult { get; private set; }
        public DataAsyncResult(T t)
        {
            DataResult = t;
        }

        public object AsyncState
        {
            get { return null; }       
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { return null; }         
        }

        public bool CompletedSynchronously
        {
            get { return false;}         
        }

        public bool IsCompleted
        {
            get { return true; }            
        }
    }

    public delegate void DataAsyncCallback<T>(DataAsyncResult<T> ar);
}
