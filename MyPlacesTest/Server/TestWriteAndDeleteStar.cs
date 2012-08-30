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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using MyPlaces.Model;
using MyPlaces.Server;

namespace MyPlacesTest.Server
{
    [TestClass]   
    public class TestWriteAndDeleteStar : WorkItemTest
    {
        static string adr = "http://myplaces.scurab.com:8182";

        [TestMethod]
        [Asynchronous]        
        public void WriteAndDeleteStar()
        {
            Star s = new Star();
            s.Note = "TestNote";
            s.Type = "20";
            s.X = 14.4;
            s.Y = 50.1;
            ServerConnection sc = new ServerConnection(adr);
            sc.Save(s, new DataAsyncCallback<Star>((res) =>
            {
                Assert.IsTrue(res.DataResult.ID > 0);
                sc.Delete(res.DataResult, new DataAsyncCallback<Star>((ia) =>
                {
                    Assert.IsNotNull(ia.DataResult);
                    Assert.IsNull(ia.Error);
                    EnqueueTestComplete();
                }));
            }));
        }
    }
}
