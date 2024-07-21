using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace IT123P___MP
{
    [Activity(Label = "createOutfit", Theme = "@style/Theme.Design")]
    public class createOutfit : Activity
    {
        ImageButton upper, lower, feet, acc1, acc2, acc3;
        Button random, next, back;
        public string upperImg, lowerImg, feetImg, acc1Img, acc2Img, acc3Img;
        string name;
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

                    Android.Graphics.Bitmap imgBm = (Android.Graphics.Bitmap)null;
                    if (fileName.Length == 0)
                    {
                        imgBm = BitmapFactory.DecodeResource(this.Resources, Android.Resource.Drawable.IcMenuGallery);
                    }
                    else
                    {
                        imgBm = FormatImage(fileName);
                    }

                    if (this.type == "upper")
                    {
                        upper.SetScaleType(ImageButton.ScaleType.CenterCrop);
                        upper.SetImageBitmap(imgBm);
                        upperImg = fileName;
                    }
                    else if (this.type == "lower")
                    {
                        lower.SetScaleType(ImageButton.ScaleType.CenterCrop);
                        lower.SetImageBitmap(imgBm);
                        lowerImg = fileName;
                    }
                    else if (this.type == "feet")
                    {
                        feet.SetScaleType(ImageButton.ScaleType.CenterCrop);
                        feet.SetImageBitmap(imgBm);
                        feetImg = fileName;
                    }
                    else if (this.type == "acc1")
                    {
                        acc1.SetScaleType(ImageButton.ScaleType.CenterCrop);
                        acc1.SetImageBitmap(imgBm);
                        acc1Img = fileName;
                    }
                    else if (this.type == "acc2")
                    {
                        acc2.SetScaleType(ImageButton.ScaleType.CenterCrop);
                        acc2.SetImageBitmap(imgBm);
                        acc2Img = fileName;
                    }
                    else if (this.type == "acc3")
                    {
                        acc3.SetScaleType(ImageButton.ScaleType.CenterCrop);
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

        public void RandomOutfit(object sender, EventArgs e)
        {
            upperImg = FetchOutfit("upper").ToString();
            lowerImg = FetchOutfit("lower").ToString();
            feetImg = FetchOutfit("feet").ToString();
            acc1Img = FetchOutfit("acc1").ToString();
            acc2Img = FetchOutfit("acc2").ToString();
            acc3Img = FetchOutfit("acc3").ToString();

            upper.SetScaleType(ImageButton.ScaleType.CenterCrop);
            upper.SetImageBitmap(FormatImage(upperImg));

            lower.SetScaleType(ImageButton.ScaleType.CenterCrop);
            lower.SetImageBitmap(FormatImage(lowerImg));

            feet.SetScaleType(ImageButton.ScaleType.CenterCrop);
            feet.SetImageBitmap(FormatImage(feetImg));

            acc1.SetScaleType(ImageButton.ScaleType.CenterCrop);
            acc1.SetImageBitmap(FormatImage(acc1Img));

            acc2.SetScaleType(ImageButton.ScaleType.CenterCrop);
            acc2.SetImageBitmap(FormatImage(acc2Img));

            acc3.SetScaleType(ImageButton.ScaleType.CenterCrop);
            acc3.SetImageBitmap(FormatImage(acc3Img));
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
            name = Intent.GetStringExtra("name");

            if (upperImg == null || lowerImg == null || feetImg == null)
            {
                Toast.MakeText(this, "Please fill out the Upper, Lower, and Feet fields",ToastLength.Short).Show();
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
                i.PutExtra("name", name);
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
            }
        }

        public Bitmap FormatImage(string img)
        {
            Android.Graphics.Bitmap bitmap;

            var imageBitmap = UtilityClass.GetImageBitmapFromUrl($"http://{local_ip}/REST/IT123P/MP/img/{img}.jpg");

            if (imageBitmap == null)
            {
                bitmap = BitmapFactory.DecodeResource(this.Resources, Android.Resource.Drawable.IcMenuGallery);
            }
            else
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