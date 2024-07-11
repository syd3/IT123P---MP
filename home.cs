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

namespace IT123P___MP
{
    [Activity(Label = "home")]
    public class home : Activity
    {
        TextView txt;
        Button viewOutfit, viewClothing, createOutfit, newClothes, logOut;
        string name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.home);

            txt = FindViewById<TextView>(Resource.Id.textView1);
            viewOutfit = FindViewById<Button>(Resource.Id.button1);
            viewClothing = FindViewById<Button>(Resource.Id.button2);
            createOutfit = FindViewById<Button>(Resource.Id.button3);
            newClothes = FindViewById<Button>(Resource.Id.button4);
            logOut = FindViewById<Button>(Resource.Id.button5);

            name = Intent.GetStringExtra("name");
            txt.Text = $"Welcome, {name}";

            viewOutfit.Click += this.ViewOutfit;
            viewClothing.Click += this.ViewClothing;
            createOutfit.Click += this.CreateOutfit;
            newClothes.Click += this.NewClothes;
            logOut.Click += this.LogOut;
        }

        public void ViewOutfit(object sender, EventArgs e)
        {

        }
        
        public void ViewClothing(object sender, EventArgs e)
        {

        }

        public void CreateOutfit(object sender, EventArgs e)
        {

        }

        public void NewClothes(object sender, EventArgs e)
        {

        }

        public void LogOut(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }
    }
}