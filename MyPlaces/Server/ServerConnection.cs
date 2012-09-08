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
        private string mMapItemsCoordsUrl;
        private string mMapItemTypesUrl;

        private const string STARS_SUFFIX = "/stars";
        private const string MAPITEMS_SUFFIX = "/mapitems";
        private const string MAP_ITEM_TYPES_SUFFIX = "/mapitemtypes";
        private const string MAPITEMS_COORDS_SUFFIX = "/{0}/{1}/{2}/{3}";

        private const string PUT = "PUT";
        private const string GET = "GET";
        private const string POST = "POST";
        private const string DELETE = "DELETE";

        public ServerConnection(string url = "http://myplaces.scurab.com:8182")
        {
            mServerUrl = url;
            mStarsUrl = mServerUrl + STARS_SUFFIX;
            mMapItemsUrl = mServerUrl + MAPITEMS_SUFFIX;
            mMapItemsCoordsUrl = mServerUrl + MAPITEMS_SUFFIX + MAPITEMS_COORDS_SUFFIX;
            mMapItemTypesUrl = mServerUrl + MAP_ITEM_TYPES_SUFFIX;
        }

        /// <summary>
        /// Downloads stars from server
        /// </summary>
        /// <param name="Callback"></param>
        public void GetStars(DataAsyncCallback<List<Star>> Callback)
        {
            WebRequest req = WebRequest.CreateHttp(mStarsUrl);
            req.Method = GET;
            req.BeginGetResponse(new AsyncCallback(
                (IAsyncResult result) =>
                {
                    List<Star> dataResult = null;
                    Exception err = null;
                    try
                    {
                        WebResponse s = req.EndGetResponse(result);
                        string json = ReadStreamToEnd(s.GetResponseStream());
                        s.Close();

                        dataResult = JsonConvert.DeserializeObject<List<Star>>(json);
                    }
                    catch (Exception e)
                    {
                        err = e;
                    }
                    finally
                    {
                        Callback(new DataAsyncResult<List<Star>>(dataResult,err));
                    }
                }), req);            
        }

        public void Save(Star s, DataAsyncCallback<Star> Callback)
        {
            WebRequest wr = WebRequest.CreateHttp(mStarsUrl);
            wr.Method = (s.ID == 0) ? POST : PUT;
            wr.BeginGetRequestStream(new AsyncCallback((iAsyncResult) =>
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(s);
                        byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
                        Stream stream = wr.EndGetRequestStream(iAsyncResult);
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                        wr.BeginGetResponse(new AsyncCallback((iAsyncResult2) =>
                            {
                                Star saved = s;
                                Exception err = null;
                                try
                                {
                                    WebResponse resp = wr.EndGetResponse(iAsyncResult2);
                                    string respText = ReadStreamToEnd(resp.GetResponseStream());
                                    resp.Close();
                                    saved = JsonConvert.DeserializeObject<Star>(respText);                                  
                                }
                                catch (Exception e)
                                {
                                    err = e;
                                }
                                finally
                                {
                                    Callback.Invoke(new DataAsyncResult<Star>(saved, err));
                                }
                            }), wr);
                    }
                    catch (Exception e)
                    {
                        Callback.Invoke(new DataAsyncResult<Star>(s, e));
                    }
                }), wr);
        }

        public void Delete(Star s, DataAsyncCallback<Star> Callback)
        {
            WebRequest wr = WebRequest.CreateHttp(mStarsUrl + "/" + s.ID);
            wr.Method = DELETE;      
            wr.BeginGetResponse(new AsyncCallback((iAsyncResult2) =>
            {
                Exception err = null;
                try
                {
                    WebResponse resp = wr.EndGetResponse(iAsyncResult2);
                    string respText = ReadStreamToEnd(resp.GetResponseStream());
                    resp.Close();
                }
                catch (Exception e)
                {
                    err = e;
                }
                finally
                {
                    Callback.Invoke(new DataAsyncResult<Star>(s, err));
                }
                
            }), wr);           
        }

        public void Delete(MapItem mi, DataAsyncCallback<MapItem> Callback)
        {
            WebRequest wr = WebRequest.CreateHttp(mMapItemsUrl + "/" + mi.ID);
            wr.Method = DELETE;
            wr.BeginGetResponse(new AsyncCallback((iAsyncResult2) =>
            {
                Exception err = null;
                try
                {
                    WebResponse resp = wr.EndGetResponse(iAsyncResult2);
                    string respText = ReadStreamToEnd(resp.GetResponseStream());
                    resp.Close();
                }
                catch (Exception e)
                {
                    err = e;
                }
                finally
                {
                    Callback.Invoke(new DataAsyncResult<MapItem>(mi, err));
                }

            }), wr);
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
            string url = String.Format(mMapItemsCoordsUrl,-180,-90,90,180);
            WebRequest req = WebRequest.CreateHttp(url);
            req.Method = "GET";
            req.BeginGetResponse(new AsyncCallback(
                (IAsyncResult result) =>
                {
                    List<MapItem> dataResult = null;
                    Exception err = null;
                    try
                    {
                        WebResponse s = req.EndGetResponse(result);
                        string json = ReadStreamToEnd(s.GetResponseStream());
                        s.Close();
                        dataResult = JsonConvert.DeserializeObject<List<MapItem>>(json);
                    }
                    catch (Exception e)
                    {
                        err = e;
                    }
                    finally
                    {
                        dataAsyncCallback(new DataAsyncResult<List<MapItem>>(dataResult,err));
                    }
                }), req);     
        }

        public void GetMapItem(string mapItemId, DataAsyncCallback<MapItem> dataAsyncCallback)
        {
            string url = String.Format(mMapItemsUrl + "/" + mapItemId);
            WebRequest req = WebRequest.CreateHttp(url);
            req.Method = "GET";
            req.BeginGetResponse(new AsyncCallback(
                (IAsyncResult result) =>
                {
                    MapItem dataResult = null;
                    Exception err = null;
                    try
                    {
                        WebResponse s = req.EndGetResponse(result);
                        string json = ReadStreamToEnd(s.GetResponseStream());
                        s.Close();
                        List<MapItem> subResult = JsonConvert.DeserializeObject<List<MapItem>>(json);
                        if (subResult.Count == 1)
                            dataResult = subResult[0];                       
                    }
                    catch (Exception e)
                    {
                        err = e;
                    }
                    finally
                    {
                        dataAsyncCallback(new DataAsyncResult<MapItem>(dataResult, err));
                    }
                }), req);
        }



        public void Save(MapItem mi, DataAsyncCallback<MapItem> Callback)
        {
            WebRequest wr = WebRequest.CreateHttp(mMapItemsUrl);
            wr.Method = (mi.ID == 0) ? POST : PUT;
            wr.BeginGetRequestStream(new AsyncCallback((iAsyncResult) =>
            {
                try
                {
                    string json = JsonConvert.SerializeObject(mi);
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
                    Stream stream = wr.EndGetRequestStream(iAsyncResult);
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    wr.BeginGetResponse(new AsyncCallback((iAsyncResult2) =>
                    {
                        MapItem saved = mi;
                        Exception err = null;
                        try
                        {
                            WebResponse resp = wr.EndGetResponse(iAsyncResult2);
                            string respText = ReadStreamToEnd(resp.GetResponseStream());
                            resp.Close();
                            saved = JsonConvert.DeserializeObject<MapItem>(respText);
                        }
                        catch (Exception e)
                        {
                            err = e;
                        }
                        finally
                        {
                            Callback.Invoke(new DataAsyncResult<MapItem>(saved, err));
                        }
                    }), wr);
                }
                catch (Exception e)
                {
                    Callback.Invoke(new DataAsyncResult<MapItem>(mi, e));
                }
            }), wr);
        }

        public void GetMapItemTypes(DataAsyncCallback<List<string>> dataAsyncCallback)
        {
            WebRequest req = WebRequest.CreateHttp(mMapItemTypesUrl);
            req.Method = "GET";
            req.BeginGetResponse(new AsyncCallback(
                (IAsyncResult result) =>
                {
                    List<string> dataResult = null;
                    Exception err = null;
                    try
                    {
                        WebResponse s = req.EndGetResponse(result);
                        string json = ReadStreamToEnd(s.GetResponseStream());
                        s.Close();
                        dataResult = JsonConvert.DeserializeObject<List<string>>(json);                        
                    }
                    catch (Exception e)
                    {
                        err = e;
                    }
                    finally
                    {
                        dataAsyncCallback(new DataAsyncResult<List<string>>(dataResult, err));
                    }
                }), req);
        }
    }

    

    public class DataAsyncResult<T> : IAsyncResult
    {
        public T DataResult { get; private set; }
        public Exception Error { get; private set; }
        public DataAsyncResult(T t)
        {
            DataResult = t;
        }

        public DataAsyncResult(T t, Exception e):this(t)
        {
            Error = e;
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
