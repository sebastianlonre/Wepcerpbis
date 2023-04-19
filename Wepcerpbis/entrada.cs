using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wepcerpbis
{
    public  class entrada
    {
        public entrada()
        {
            userId = 1;
            id = 0;
            tittle = "";
            body = "";
        }

        public int userId { get; set; }
        public int id { get; set; }
        public string tittle { get; set; }
        public string body { get; set; }
    }
}