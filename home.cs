using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace IT123P___MP
{
    [Activity(Label = "home", Theme = "@style/Theme.Design")]
    public class home : Activity
    {
        Button viewOutfit, viewClothing, createOutfit, newClothes, logOut;
        TextView txt;
        public string name;

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

            logOut.Click += LogOut_Click;

        }
        public void LogOut()
        {
            Intent i = new Intent(this, typeof(MainActivity));
            i.SetFlags(ActivityFlags.ClearTop);
            StartActivity(i);
            Finish(); // Ensures that this current activity will not be running in the background afterwards
        }
        
        public void LogOut_Click(object sender, EventArgs e)
        {
            LogOut();
        }
        
        public override void OnBackPressed()
        {
            LogOut();
        }

        public void ViewOutfit(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(viewOutfit));
            i.PutExtra("name", name);
            StartActivity(i);
        }
        
        public void ViewClothing(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(viewClothing));
            StartActivity(i);
        }

        public void CreateOutfit(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(createOutfit));
            i.PutExtra("name", name);
            StartActivity(i);
        }

        public void NewClothes(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(New_Clothes));
            StartActivity(i);
        }
    }
}