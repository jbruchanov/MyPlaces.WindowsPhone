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

namespace MyPlaces.Model
{
    public class Star
    {
        public long id {get;set;}
        public string note {get;set;}
        public string type {get;set;}
        public double x {get;set;}
        public double y {get;set;}
    }
}
