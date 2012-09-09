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
using Coding4Fun.Phone.Controls;

namespace MyPlaces
{
    public partial class App
    {
        public static string MAP_ITEM_ID
        {
            get
            {
                return "MAP_ITEM_ID";
            }
        }
        public static string X
        {
            get
            {
                return "X";
            }
        }

        public static string Y
        {
            get
            {
                return "Y";
            }
        }

        public static string HTTP_PREFIX
        {

            get
            {
                return "http://";
            }
        }

        public static void ShowToast(string msg, string title = null)
        {
            ToastPrompt tp = new ToastPrompt();
            tp.Title = title;
            tp.Message = msg;
            tp.Show();
        }

        public static void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
