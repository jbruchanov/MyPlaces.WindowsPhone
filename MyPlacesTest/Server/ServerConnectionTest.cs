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
using System.Collections.Generic;
using MyPlaces.Model;
using MyPlaces.Server;
using Microsoft.Silverlight.Testing;

namespace MyPlacesTest.Server
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ServerConnectionTest : WorkItemTest
    {
        private string adr = "http://myplaces.scurab.com:8182";

        [TestMethod, Asynchronous]       
        public void TestGetStars()
        {
            ServerConnection sc = new ServerConnection(adr);
            sc.GetStars(new DataAsyncCallback<List<Star>>((result)
                =>
                {
                    List<Star> stars = result.DataResult;
                    Assert.IsNotNull(stars);
                    Assert.IsTrue(stars.Count > 0);
                    EnqueueTestComplete();
                }));
        }

        [TestMethod, Asynchronous]
        public void TestGetMapItems()
        {
            ServerConnection sc = new ServerConnection(adr);
            sc.GetMapItems(new DataAsyncCallback<List<MapItem>>((result)
                =>
            {
                List<MapItem> stars = result.DataResult;
                Assert.IsNotNull(stars);
                Assert.IsTrue(stars.Count > 0);
                EnqueueTestComplete();
            }));
        }


    }

}
