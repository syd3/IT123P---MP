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
        string id;
        string Name;
        string Description;
        string upper;
        string lower;
        string feet;
        string Occasion;
        string acc1;
        string acc2;
        string acc3;
        string owner;

        public string outfit_id { get { return this.id; } set { this.id = value; } }
        public string name { get { return this.Name; } set { this.Name= value; } }
        public string description { get { return this.Description; } set { this.Description = value; } }
        public string upper_outfit_id { get { return this.upper; } set { this.upper = value; } }
        public string lower_outfit_id { get { return this.lower; } set { this.lower = value; } }
        public string feet_outfit_id { get { return this.feet; } set { this.feet = value; } }
        public string occasion { get { return this.Occasion; } set { this.Occasion = value; } }
        public string accessory_id_1 { get { return this.acc1; } set { this.acc1 = value; } }
        public string accessory_id_2 { get { return this.acc2; } set { this.acc2 = value; } }
        public string accessory_id_3 { get { return this.acc3; } set { this.acc3 = value; } }
        public string owner_id { get { return this.owner; } set { this.owner = value; } }

        public void Shout(TextView v)
        {
            v.Text += "ID:" + this.id + ",";
            v.Text += "Name:" + this.Name + ",";
            v.Text += "Des: " + this.Description + ",";
            v.Text += "Up:" + this.upper + ",";
            v.Text += "Low:" + this.lower + ",";
            v.Text += "Feet:" + this.feet + ",";
            v.Text += "Occasion:" + this.occasion + ",";
            v.Text += "Ac1:" + this.acc1 + ",";
            v.Text += "Ac2:" + this.acc2 + ",";
            v.Text += "Ac3:" + this.acc3 + ",";
            v.Text += "\n\n";
        }
    }
}