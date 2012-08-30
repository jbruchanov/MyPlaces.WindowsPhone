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
using MyPlaces.Server;
using Microsoft.Silverlight.Testing;
using MyPlaces.Model;
using System.Collections.Generic;

namespace MyPlacesTest.Server
{
    [TestClass]
    public class TestGetMapItems : WorkItemTest
    {
        private static string adr = "http://myplaces.scurab.com:8182";

        [TestMethod]
        [Asynchronous]
        public void GetMapItems()
        {
            ServerConnection sc = new ServerConnection(adr);
            sc.GetMapItems(new DataAsyncCallback<List<MapItem>>((result)
                =>
            {
                List<MapItem> stars = result.DataResult;
                Assert.IsNotNull(stars);
                Assert.IsNull(result.Error);
                Assert.IsTrue(stars.Count > 0);
                EnqueueTestComplete();
            }));
            //EnqueueDelay(2000);
        }
    }
}
