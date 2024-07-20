using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Text.Json;
using Android.Graphics;
using Java.Interop;
using Java.IO;

namespace IT123P___MP
{
    [Activity(Label = "ViewOutfits")]
    public class ViewOutfits : Activity
    {
        Button back_btn;
        View container;
        HttpWebRequest request;
        HttpWebResponse response;
        int requestCode;
        OutputStream outputstream;
        ImageView iv;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_outfits);
            back_btn = FindViewById<Button>(Resource.Id.back_btn);
            back_btn.Click += delegate
            {
                Intent i = new Intent(this, typeof(home));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };

            container = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            //container = FindViewById<LinearLayout>(Resource.Id.outfit_container);

            request = (HttpWebRequest)WebRequest.Create($"http://192.168.100.63/REST/IT123P/MP/API/fetch_outfit.php");
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String res = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(res); // Parse the web response since the API returns a Json object
            JsonElement root = doc.RootElement; // This will return an array
            Outfit[] outfits = JsonSerializer.Deserialize<Outfit[]>(doc);
            foreach (Outfit o in outfits) {; }
        }
    }
}