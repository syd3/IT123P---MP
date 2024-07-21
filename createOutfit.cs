using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace IT123P___MP
{
    [Activity(Label = "createOutfit")]
    public class createOutfit : Activity
    {
        ImageButton upper, lower, feet, acc1, acc2, acc3;
        Button random, next, back;
        public string upperImg, lowerImg, feetImg, acc1Img, acc2Img, acc3Img;
        int requestCode;
        string type, res;
        string local_ip = UtilityClass.ip;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.create_outfit);

            upper = FindViewById<ImageButton>(Resource.Id.upper_button);
            lower = FindViewById<ImageButton>(Resource.Id.lower_button);
            feet = FindViewById<ImageButton>(Resource.Id.feet_button);
            acc1 = FindViewById<ImageButton>(Resource.Id.accessory1_button);
            acc2 = FindViewById<ImageButton>(Resource.Id.accessory2_button);
            acc3 = FindViewById<ImageButton>(Resource.Id.accessory3_button);
            random = FindViewById<Button>(Resource.Id.button1);
            next = FindViewById<Button>(Resource.Id.button2);
            back = FindViewById<Button>(Resource.Id.button3);

            upper.Click += this.OutfitSelection;
            lower.Click += this.OutfitSelection;
            feet.Click += this.OutfitSelection;
            acc1.Click += this.OutfitSelection;
            acc2.Click += this.OutfitSelection;
            acc3.Click += this.OutfitSelection;
            random.Click += this.RandomOutfit;
            next.Click += this.CreateOutfitFinal;

            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(home));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };
        }

        // Gets the result from the OutfitSelection method and sets the respective values
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (data != null && data.HasExtra("fileName"))
                {
                    string fileName = data.GetStringExtra("fileName");

                    // Retrieves an image from a URL
                    Android.Graphics.Bitmap imgBm = (Android.Graphics.Bitmap)null;
                    if (fileName.Length == 0)
                    {
                        imgBm = BitmapFactory.DecodeResource(this.Resources, Android.Resource.Drawable.IcMenuGallery);
                    }
                    else
                    {
                        string local_ip = UtilityClass.ip;
                        imgBm = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{fileName}.jpg");
                    }

                    // Code below can probably be improved more, if possible find way to loop it instead
                    if (this.type == "upper")
                    {
                        upper.SetImageBitmap(imgBm);
                        upperImg = fileName;
                    }
                    else if (this.type == "lower")
                    {
                        lower.SetImageBitmap(imgBm);
                        lowerImg = fileName;
                    }
                    else if (this.type == "feet")
                    {
                        feet.SetImageBitmap(imgBm);
                        feetImg = fileName;
                    }
                    else if (this.type == "acc1")
                    {
                        acc1.SetImageBitmap(imgBm);
                        acc1Img = fileName;
                    }
                    else if (this.type == "acc2")
                    {
                        acc2.SetImageBitmap(imgBm);
                        acc2Img = fileName;
                    }
                    else if (this.type == "acc3")
                    {
                        acc3.SetImageBitmap(imgBm);
                        acc3Img = fileName;
                    }
                }
            }
        }

        public void OutfitSelection(object sender, EventArgs e)
        {
            ImageButton imbutton = (ImageButton)sender;
            int type = imbutton.Id;
            string outfitType = "";

            // Code below can probably be improved more, if possible find way to loop it instead
            // Converts the int of a widget object into its respective string value
            if (type == Resource.Id.upper_button)
            {
                outfitType = "upper";
                this.type = "upper";
            } else if (type == Resource.Id.lower_button)
            {
                outfitType = "lower";
                this.type = "lower";
            }
            else if (type == Resource.Id.feet_button)
            {
                outfitType = "feet";
                this.type = "feet";
            } else if (type == Resource.Id.accessory1_button)
            {
                outfitType = "acc1";
                this.type = "acc1";
            } else if (type == Resource.Id.accessory2_button)
            {
                outfitType = "acc2";
                this.type = "acc2";
            } else if (type == Resource.Id.accessory3_button)
            {
                outfitType = "acc3";
                this.type = "acc3";
            }

            Intent i = new Intent(this, typeof(createOutfitSelection));
            i.PutExtra("type", outfitType);
            StartActivityForResult(i, requestCode);
        }

        public void RandomOutfit(object sender, EventArgs e) // Crashes when used for a second time? Or maybe a problem with your phone
        {
            upperImg = FetchOutfit("upper").ToString();
            lowerImg = FetchOutfit("lower").ToString();
            feetImg = FetchOutfit("feet").ToString();
            acc1Img = FetchOutfit("acc1").ToString();
            acc2Img = FetchOutfit("acc2").ToString();
            acc3Img = FetchOutfit("acc3").ToString();

            Bitmap up  = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{upperImg}.jpg");
            Bitmap low = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{lowerImg}.jpg");
            Bitmap ft = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{feetImg}.jpg");
            Bitmap ac1 = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{acc1Img}.jpg");
            Bitmap ac2 = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{acc2Img}.jpg");
            Bitmap ac3 = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{acc3Img}.jpg");
            upper.SetImageBitmap(up);
            lower.SetImageBitmap(low);
            feet.SetImageBitmap(ft);
            acc1.SetImageBitmap(ac1);
            acc2.SetImageBitmap(ac2);
            acc3.SetImageBitmap(ac3);

            up.Dispose();
            low.Dispose();
            ft.Dispose();
            ac1.Dispose();
            ac2.Dispose();
            ac3.Dispose();
        }

        public string FetchOutfit(string outfitName)
        {
            request = (HttpWebRequest)WebRequest.Create($"http://{local_ip}/REST/IT123P/MP/API/fetch_clothes.php?type={outfitName}");
            response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            { res = reader.ReadToEnd(); }
            JsonElement root;
            JsonElement element;
            using (JsonDocument doc = JsonDocument.Parse(res))
            {
                root = doc.RootElement;
                element = root.Clone();
            }

            response.Dispose();
            response.Close();
            request.Abort();
            request = null;

            Random rnd = new Random();
            int randomClothe = rnd.Next(0, element.GetArrayLength());

            return element[randomClothe].ToString();
        }

        public void CreateOutfitFinal(object sender, EventArgs e)
        {
            string name = Intent.GetStringExtra("name");

            // Ensure first that the required fields such as upper, lower, and feet have values before proceeding
            if (upperImg == null || lowerImg == null || feetImg == null)
            {
                Toast.MakeText(this, "Please fill out Upper, Lower, and Feet categories.",ToastLength.Short).Show();
            }
            else
            {
                Intent i = new Intent(this, typeof(createOutfit2));
                i.PutExtra("name", name);
                i.PutExtra("upperImg", upperImg); // Required
                i.PutExtra("lowerImg", lowerImg); // Required
                i.PutExtra("feetImg", feetImg); // Required
                i.PutExtra("acc1Img", acc1Img);
                i.PutExtra("acc2Img", acc2Img);
                i.PutExtra("acc3Img", acc3Img);
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
            }
        }
    }
}