using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.IO;
using System.Net;
using System.Text.Json;

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

            var imageButtons = new ImageButton[]
            {
                imb1 = FindViewById<ImageButton>(Resource.Id.imageButton1),
                imb2 = FindViewById<ImageButton>(Resource.Id.imageButton2),
                imb3 = FindViewById<ImageButton>(Resource.Id.imageButton3),
                imb4 = FindViewById<ImageButton>(Resource.Id.imageButton4),
                imb5 = FindViewById<ImageButton>(Resource.Id.imageButton5)
            };

            back = FindViewById<Button>(Resource.Id.button1);

            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(createOutfit));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };

            string type = Intent.GetStringExtra("type");

            request = (HttpWebRequest)WebRequest.Create($"http://192.168.1.14/REST/IT123P/MP/API/fetch_clothes.php?type={type}");
            response = (HttpWebResponse)request.GetResponse(); // Web Request to retrieve the file name of the images
            StreamReader reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(res); // Parse the web response since the API returns a Json object
            JsonElement root = doc.RootElement; // This will return an array

            // Loops through each file name found and sets the appropriate image for each imagebutton
            for (int i = 0; i < root.GetArrayLength(); i++)
            {
                JsonElement element = root[i].Clone(); // Needed so that the Json Object is still accessible after being disposed

                var imageBitmap = UtilityClass.GetImageBitmapFromUrl($"http://192.168.1.14/REST/IT123P/MP/img/{root[i]}.jpg");
                imageButtons[i].SetImageBitmap(imageBitmap);
                imageButtons[i].Click += delegate
                {
                    Intent t = new Intent(this, typeof(createOutfit));
                    t.PutExtra("fileName", element.ToString());
                    t.PutExtra("type", type);
                    t.SetFlags(ActivityFlags.ReorderToFront);
                    SetResult(Result.Ok, t); // Ensures that the appropriate data will be sent back
                    StartActivity(t);
                    Finish();
                };
            }
        }
    }
}