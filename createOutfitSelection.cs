using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.IO;
using System.Net;
using System.Text.Json;
using Android.Views;
using Google.Android.Material.Internal;
using System.Drawing;
using Android.Graphics;
using System.Runtime.Remoting.Contexts;

namespace IT123P___MP
{
    [Activity(Label = "createOutfitSelection")]
    public class createOutfitSelection : Activity
    {
        ImageButton imb1, imb2, imb3, imb4, imb5;
        Button back;
        string res;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.create_outfit_selection);

            back = FindViewById<Button>(Resource.Id.button1);
            FlowLayout container = (FlowLayout)FindViewById<View>(Resource.Id.clothes_container);

            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(createOutfit));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };

            string type = Intent.GetStringExtra("type");

            request = (HttpWebRequest)WebRequest.Create($"http://192.168.100.63/REST/IT123P/MP/API/fetch_clothes.php?type={type}");
            response = (HttpWebResponse)request.GetResponse(); // Web Request to retrieve the file name of the images
            StreamReader reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(res); // Parse the web response since the API returns a Json object
            JsonElement root = doc.RootElement; // This will return an array

            // Loops through each file name found and sets the appropriate image for each imagebutton
            for (int i = 0; i < root.GetArrayLength(); i++)
            {
                JsonElement element = root[i].Clone(); // Needed so that the Json Object is still accessible after being disposed

                var imageBitmap = UtilityClass.GetImageBitmapFromUrl($"http://192.168.100.63/REST/IT123P/MP/img/{root[i]}.jpg");
                //imageButtons[i].SetImageBitmap(imageBitmap);
                //imageButtons[i].Click += delegate

                Display d = this.WindowManager.DefaultDisplay;
                Android.Util.DisplayMetrics m = new Android.Util.DisplayMetrics();
                d.GetMetrics(m);

                int width = (int)((m.WidthPixels - 8) / 2);
                int height = (int)(120 * m.Density);
                int newHeight = (int)((float)width/(float)imageBitmap.Width * imageBitmap.Height);
                int y = (newHeight - height) / 2;
                //Rectangle rec = new Rectangle(0, y, width, height);
                imageBitmap = Android.Graphics.Bitmap.CreateScaledBitmap(imageBitmap, width, newHeight, true);
                Android.Graphics.Bitmap bitmap = Android.Graphics.Bitmap.CreateBitmap(imageBitmap, 0, y, width, height);

                ImageView child = (ImageView)LayoutInflater.Inflate(Resource.Layout.clothe_ll, null);
                child.SetScaleType(ImageView.ScaleType.CenterCrop);

                child.SetImageBitmap(bitmap);
                child.Click += delegate
                {
                    Intent t = new Intent(this, typeof(createOutfit));
                    t.PutExtra("fileName", element.ToString());
                    t.PutExtra("type", type);
                    t.SetFlags(ActivityFlags.ReorderToFront);
                    SetResult(Result.Ok, t); // Ensures that the appropriate data will be sent back
                    StartActivity(t);
                    Finish();
                };

                container.AddView(child);

            }
        }
    }
}