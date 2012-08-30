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
    public class TestWriteAndMapItem : WorkItemTest
    {
        static string adr = "http://myplaces.scurab.com:8182";

        [TestMethod]
        [Asynchronous]
        [Tag("now")]
        public void WriteAndDeleteStar()
        {
            MapItem mi = new MapItem
            {
                Name = "ABC",
                City = "London",
                Street = "Street",
                Country = "UK",
                Type = "Hospoda",
                X = 3,
                Y = 52,
                
            };

            ServerConnection sc = new ServerConnection(adr);
            sc.Save(mi, new DataAsyncCallback<MapItem>((res) =>
            {
                Assert.IsTrue(res.DataResult.ID > 0);
                sc.Delete(res.DataResult, new DataAsyncCallback<MapItem>((ia) =>
                {
                    Assert.IsNotNull(ia.DataResult);
                    Assert.IsNull(ia.Error);
                    EnqueueTestComplete();
                }));
            }));
        }
    }
}
