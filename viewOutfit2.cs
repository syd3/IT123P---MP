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
using System.Net;
using System.IO;
using System.Text.Json;

namespace IT123P___MP
{
    [Activity(Label = "viewOutfit2")]
    public class viewOutfit2 : Activity
    {
        ImageView upperImg, lowerImg, feetImg, acc1Img, acc2Img, acc3Img;
        TextView outfit, occasion, desc;
        Button back;
        string res;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_outfits_2);

            //var imgVw = new ImageView[]
            //{
            //    upperImg = FindViewById<ImageView>(Resource.Id.imageView1),
            //    lowerImg = FindViewById<ImageView>(Resource.Id.imageView3),
            //    feetImg = FindViewById<ImageView>(Resource.Id.imageView5),
            //    acc1Img = FindViewById<ImageView>(Resource.Id.imageView2),
            //    acc2Img = FindViewById<ImageView>(Resource.Id.imageView4),
            //    acc3Img = FindViewById<ImageView>(Resource.Id.imageView6)
            //};

            upperImg = FindViewById<ImageView>(Resource.Id.imageView1);
            lowerImg = FindViewById<ImageView>(Resource.Id.imageView3);
            feetImg = FindViewById<ImageView>(Resource.Id.imageView5);
            acc1Img = FindViewById<ImageView>(Resource.Id.imageView2);
            acc2Img = FindViewById<ImageView>(Resource.Id.imageView4);
            acc3Img = FindViewById<ImageView>(Resource.Id.imageView6);

            outfit = FindViewById<TextView>(Resource.Id.textView2);
            occasion = FindViewById<TextView>(Resource.Id.textView3);
            desc = FindViewById<TextView>(Resource.Id.textView4);
            back = FindViewById<Button>(Resource.Id.button1);

            string outfitName = Intent.GetStringExtra("outfitName");
            string name = Intent.GetStringExtra("name");

            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(viewOutfit));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
            };

            string local_ip = UtilityClass.ip;
            request = (HttpWebRequest)WebRequest.Create($"http://{local_ip}/REST/IT123P/MP/API/fetch_outfits_2.php?user={name}&name={outfitName}");
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(res);
            JsonElement root = doc.RootElement;

            outfit.Text = root[0].ToString();
            desc.Text = root[1].ToString();
            occasion.Text = root[2].ToString();

            upperImg.SetImageBitmap(UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[3]}.jpg"));
            lowerImg.SetImageBitmap(UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[4]}.jpg"));
            feetImg.SetImageBitmap(UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[5]}.jpg"));
            //acc1Img.SetImageBitmap(UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[6]}.jpg"));
            //acc2Img.SetImageBitmap(UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[7]}.jpg"));
            //acc3Img.SetImageBitmap(UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[8]}.jpg"));

            //for (int i = 3; i <= 8; i++)
            //{
            //    if (root[i].ToString() != "")
            //    {
            //        var imgView = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[i]}.jpg");
            //        imgVw[i - 2].SetImageBitmap(imgView);
            //    }
            //}
        }
    }
}