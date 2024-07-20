using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.Json;

namespace IT123P___MP
{
    [Activity(Label = "viewClothing2")]
    public class viewClothing2 : Activity
    {
        TextView clothingType;
        Button back;
        string res;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_clothing_2);

            clothingType = FindViewById<TextView>(Resource.Id.textView2);
            back = FindViewById<Button>(Resource.Id.button1);

            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(viewClothing));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };

            string type = Intent.GetStringExtra("type");
            if (type == "upper")
            {
                ShowClothing("upper");
            } else if (type == "lower")
            {
                ShowClothing("lower");
            } else if (type == "feet")
            {
                ShowClothing("feet");
            } else if (type == "acc")
            {
                // Include acc2 and acc3
                ShowClothing("acc1");
            }
        }

        public void ShowClothing(string clothing)
        {
            FlowLayout container = FindViewById<FlowLayout>(Resource.Id.clothes_container);

            string local_ip = UtilityClass.ip;
            request = (HttpWebRequest)WebRequest.Create($"http://{local_ip}/REST/IT123P/MP/API/fetch_clothes.php?type={clothing}");
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(res);
            JsonElement root = doc.RootElement;

            for (int i = 0; i < root.GetArrayLength(); i++)
            {
                var imageBitmap = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[i]}.jpg");

                Display d = this.WindowManager.DefaultDisplay;
                Android.Util.DisplayMetrics m = new Android.Util.DisplayMetrics();
                d.GetMetrics(m);

                int w = (int)((m.WidthPixels - 8) / 2);
                int h = (int)(120 * m.Density);
                float ratio = (float)h / w;
                int originalw = imageBitmap.Width;
                int originalh = imageBitmap.Height;
                float imageratio = (float)originalh / (float)originalw;
                Android.Graphics.Bitmap bitmap = imageBitmap;
                if (imageratio > ratio)
                {
                    int newh = (int)((float)w / (float)imageBitmap.Width * imageBitmap.Height);
                    int y = Math.Max(0, (newh - h) / 2);
                    imageBitmap = Android.Graphics.Bitmap.CreateScaledBitmap(imageBitmap, w, newh, true);
                    bitmap = Android.Graphics.Bitmap.CreateBitmap(imageBitmap, 0, y, w, h);
                }
                else
                {
                    int neww = (int)((float)h / (float)originalh * originalw);
                    int x = Math.Max(0, (neww - w) / 2);
                    imageBitmap = Android.Graphics.Bitmap.CreateScaledBitmap(imageBitmap, neww, h, true);
                    bitmap = Android.Graphics.Bitmap.CreateBitmap(imageBitmap, x, 0, w, h);
                }

                ImageView child = (ImageView)LayoutInflater.Inflate(Resource.Layout.clothe_imgbtn, null);
                child.SetScaleType(ImageView.ScaleType.CenterCrop);
                child.SetImageBitmap(bitmap);
                container.AddView(child);
            }
        }
    }
}