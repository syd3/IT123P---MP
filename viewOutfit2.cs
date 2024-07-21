using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Net;
using System.IO;
using System.Text.Json;
using Android.Graphics;

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

            if (UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[6]}.jpg") != null)
            {
                acc1Img.SetImageBitmap(UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[6]}.jpg"));
            }
            else
            {
                acc1Img.SetImageBitmap(BitmapFactory.DecodeResource(this.Resources, Android.Resource.Drawable.IcMenuGallery));
            }

            if (UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[7]}.jpg") != null)
            {
                acc2Img.SetImageBitmap(UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[7]}.jpg"));
            }
            else
            {
                acc2Img.SetImageBitmap(BitmapFactory.DecodeResource(this.Resources, Android.Resource.Drawable.IcMenuGallery));
            }

            if (UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[8]}.jpg") != null)
            {
                acc3Img.SetImageBitmap(UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{root[8]}.jpg"));
            }
            else
            {
                acc3Img.SetImageBitmap(BitmapFactory.DecodeResource(this.Resources, Android.Resource.Drawable.IcMenuGallery));
            }
        }
    }
}