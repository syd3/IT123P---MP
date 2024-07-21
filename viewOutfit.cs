using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Internal;
using System.IO;
using System.Net;
using System.Text.Json;

namespace IT123P___MP
{
    [Activity(Label = "viewOutfit", Theme = "@style/Theme.Design")]
    public class viewOutfit : Activity
    {
        Button back;
        string res;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_outfits);

            FlowLayout container = FindViewById<FlowLayout>(Resource.Id.clothes_container);
            
            back = FindViewById<Button>(Resource.Id.button5);

            string name = Intent.GetStringExtra("name");

            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(home));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };
            
            string local_ip = UtilityClass.ip;
            request = (HttpWebRequest)WebRequest.Create($"http://{local_ip}/REST/IT123P/MP/API/fetch_outfits.php?user={name}");
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(res);
            JsonElement root = doc.RootElement;

            for (int i = 0; i < root.GetArrayLength(); i++)
            {
                JsonElement element = root[i].Clone();

                Button child = (Button)LayoutInflater.Inflate(Resource.Layout.outfit_button, null);

                child.Text = element.ToString();
                child.Click += delegate
                {
                    Intent i = new Intent(this, typeof(viewOutfit2));
                    i.PutExtra("outfitName", element.ToString());
                    i.PutExtra("name", name);
                    StartActivity(i);
                };

                container.AddView(child);
            }
        }
    }
}