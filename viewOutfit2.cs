using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;
using System.Text.Json;
using Android.Graphics;
using System;

namespace IT123P___MP
{
    [Activity(Label = "viewOutfit2", Theme = "@style/Theme.Design")]
    public class viewOutfit2 : Activity
    {
        ImageView upperImg, lowerImg, feetImg, acc1Img, acc2Img, acc3Img;
        TextView outfit, occasion, desc;
        Button back, drop;
        string res;
        string local_ip = UtilityClass.ip;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_outfits_2);

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
            drop = FindViewById<Button>(Resource.Id.del_btn);

            string outfitName = Intent.GetStringExtra("outfitName");
            string name = Intent.GetStringExtra("name");

            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(viewOutfit));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };

            drop.Click += delegate
            {
                string local_ip = UtilityClass.ip;
                request = (HttpWebRequest)WebRequest.Create($"http://{local_ip}/REST/IT123P/MP/API/drop_outfit.php?user={name}&name={outfitName}");
                response = (HttpWebResponse)request.GetResponse();

                response.Close();
                response.Dispose();
                request.Abort();
                request = null;
                response = null;

                Intent i = new Intent(this, typeof(home));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };

            request = (HttpWebRequest)WebRequest.Create($"http://{local_ip}/REST/IT123P/MP/API/fetch_outfits_2.php?user={name}&name={outfitName}");
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(res);
            JsonElement root = doc.RootElement;

            outfit.Text = root[0].ToString();
            occasion.Text = root[2].ToString();

            if (root[1].ToString() == "")
            {
                desc.Text = "No description";
            } 
            else
            {
                desc.Text = root[1].ToString();
            }

            upperImg.SetScaleType(ImageView.ScaleType.CenterCrop);
            upperImg.SetImageBitmap(FormatImage(root[3].ToString()));

            lowerImg.SetScaleType(ImageView.ScaleType.CenterCrop);
            lowerImg.SetImageBitmap(FormatImage(root[4].ToString()));

            feetImg.SetScaleType(ImageView.ScaleType.CenterCrop);
            feetImg.SetImageBitmap(FormatImage(root[5].ToString()));

            acc1Img.SetScaleType(ImageView.ScaleType.CenterCrop);
            acc1Img.SetImageBitmap(FormatImage(root[6].ToString()));

            acc2Img.SetScaleType(ImageView.ScaleType.CenterCrop);
            acc2Img.SetImageBitmap(FormatImage(root[7].ToString()));

            acc3Img.SetScaleType(ImageView.ScaleType.CenterCrop);
            acc3Img.SetImageBitmap(FormatImage(root[8].ToString()));

            response.Close();
            response.Dispose();
            request.Abort();
            request = null;
            response = null;
        }

        public Bitmap FormatImage(string img)
        {
            Android.Graphics.Bitmap bitmap;
            
            var imageBitmap = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{img}.jpg");
            
            if (imageBitmap == null)
            {
                bitmap = BitmapFactory.DecodeResource(this.Resources, Android.Resource.Drawable.IcMenuGallery);
            } else
            {
                Display d = this.WindowManager.DefaultDisplay;
                Android.Util.DisplayMetrics m = new Android.Util.DisplayMetrics();
                d.GetMetrics(m);

                int w = (int)((m.WidthPixels - 8) / 2);
                int h = (int)(120 * m.Density);
                float ratio = (float)h / w;
                int originalw = imageBitmap.Width;
                int originalh = imageBitmap.Height;
                float imageratio = (float)originalh / (float)originalw;
                bitmap = imageBitmap;
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
            }
            return bitmap;
        }
    }
}