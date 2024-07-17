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
using System.Text.Json;
using System.Text.Json.Nodes;

namespace IT123P___MP
{
    internal class Outfit
    {
        string upper;
        string lower;
        string feet;
        string acc1;
        string acc2;
        string acc3;

        public string Upper_Filename { get { return this.upper; } set { this.upper = value; } }
        public string Lower_Filename { get { return this.lower; } set { this.lower = value; } }
        public string Feet_Filename{ get { return this.feet; } set { this.lower = feet; } }
        public string Acc1_Filename { get { return this.acc1; } set { this.lower = acc1; } }
        public string Acc2_Filename { get { return this.acc2; } set { this.lower = acc2; } }
        public string Acc3_Filename{ get { return this.acc3; } set { this.lower = acc3; } }

        public Outfit(JsonElement j)
        {
            
        }
    }
}