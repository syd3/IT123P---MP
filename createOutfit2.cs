﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.IO;
using System.Net;

namespace IT123P___MP
{
    [Activity(Label = "createOutfit2", Theme = "@style/Theme.Design")]
    public class createOutfit2 : Activity
    {
        EditText outfitName, desc;
        Spinner occasion;
        Button back, create;
        string upperImg, lowerImg, feetImg, acc1Img, acc2Img, acc3Img;
        string name, res;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.create_outfit_2);

            outfitName = FindViewById<EditText>(Resource.Id.editText1);
            occasion = FindViewById<Spinner>(Resource.Id.occ_spin);
            desc = FindViewById<EditText>(Resource.Id.editText3);
            back = FindViewById<Button>(Resource.Id.button1);
            create = FindViewById<Button>(Resource.Id.button2);

            name = Intent.GetStringExtra("name");
            upperImg = Intent.GetStringExtra("upperImg");
            lowerImg = Intent.GetStringExtra("lowerImg");
            feetImg = Intent.GetStringExtra("feetImg");
            acc1Img = Intent.GetStringExtra("acc1Img");
            acc2Img = Intent.GetStringExtra("acc2Img");
            acc3Img = Intent.GetStringExtra("acc3Img");

            create.Click += this.SaveOutfit;

            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(createOutfit));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
            };
        }

        public void SaveOutfit(object sender, EventArgs e)
        {
            if (outfitName.Text == "")
            {
                Toast.MakeText(this, "Please fill out the outfit name field", ToastLength.Short).Show();
            } else
            {
                string local_ip = UtilityClass.ip;
                string url = $"http://{local_ip}/REST/IT123P/MP/API/save_outfit.php?user={name}&name={outfitName.Text}&occ={occasion.SelectedItem.ToString()}&desc={desc.Text}&upper={upperImg}&lower={lowerImg}&feet={feetImg}&acc1={acc1Img}&acc2={acc2Img}&acc3={acc3Img}";
                request = (HttpWebRequest)WebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                res = reader.ReadToEnd();

                if (res.Contains("Outfit Created"))
                {
                    Toast.MakeText(this, "Outfit Saved", ToastLength.Long).Show();
                    Intent i = new Intent(this, typeof(home));
                    i.SetFlags(ActivityFlags.ReorderToFront);
                    i.SetFlags(ActivityFlags.ClearTop);
                    i.PutExtra("name", name);
                    StartActivity(i);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Failed to create outfit", ToastLength.Short).Show();
                }

                reader.Dispose();
                response.Close();
                response.Dispose();
            }
        }
    }
}