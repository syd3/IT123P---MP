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
    [Activity(Label = "viewClothing")]
    public class viewClothing : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Button upper, lower, feet, acc, back;

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_clothing);

            upper = FindViewById<Button>(Resource.Id.button1);
            lower = FindViewById<Button>(Resource.Id.button2);
            feet = FindViewById<Button>(Resource.Id.button3);
            acc = FindViewById<Button>(Resource.Id.button4);
            back = FindViewById<Button>(Resource.Id.button5);

            upper.Click += delegate
            {
                Intent i = new Intent(this, typeof(viewClothing2));
                i.PutExtra("type", "upper");
                StartActivity(i);
            };

            lower.Click += delegate
            {
                Intent i = new Intent(this, typeof(viewClothing2));
                i.PutExtra("type", "lower");
                StartActivity(i);
            };

            feet.Click += delegate
            {
                Intent i = new Intent(this, typeof(viewClothing2));
                i.PutExtra("type", "feet");
                StartActivity(i);
            };

            acc.Click += delegate
            {
                Intent i = new Intent(this, typeof(viewClothing2));
                i.PutExtra("type", "acc");
                StartActivity(i);
            };
            
            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(home));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };
        }
    }
}