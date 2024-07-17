using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace IT123P___MP
{
    [Activity(Label = "createOutfit")]
    public class createOutfit : Activity
    {
        ImageButton upper, lower, feet, acc1, acc2, acc3;
        Button random, next, back;
        public string upperImg, lowerImg, feetImg, acc1Img, acc2Img, acc3Img;
        int requestCode;

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
                    string type = data.GetStringExtra("type");

                    // Retrieves an image from a URL
                    var imgBm = UtilityClass.GetImageBitmapFromUrl($"http://192.168.100.63/REST/IT123P/MP/img/{fileName}.jpg");

                    // Code below can probably be improved more, if possible find way to loop it instead
                    if (type == "upper")
                    {
                        upper.SetImageBitmap(imgBm);
                        upperImg = fileName;
                    }
                    else if (type == "lower")
                    {
                        lower.SetImageBitmap(imgBm);
                        lowerImg = fileName;
                    }
                    else if (type == "feet")
                    {
                        feet.SetImageBitmap(imgBm);
                        feetImg = fileName;
                    }
                    else if (type == "acc1")
                    {
                        acc1.SetImageBitmap(imgBm);
                        acc1Img = fileName;
                    }
                    else if (type == "acc2")
                    {
                        acc2.SetImageBitmap(imgBm);
                        acc2Img = fileName;
                    }
                    else if (type == "acc3")
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
            } else if (type == Resource.Id.lower_button)
            {
                outfitType = "lower";
            } else if (type == Resource.Id.feet_button)
            {
                outfitType = "feet";
            } else if (type == Resource.Id.accessory1_button)
            {
                outfitType = "acc1";
            } else if (type == Resource.Id.accessory2_button)
            {
                outfitType = "acc2";
            } else if (type == Resource.Id.accessory3_button)
            {
                outfitType = "acc3";
            } 
            
            Intent i = new Intent(this, typeof(createOutfitSelection));
            i.PutExtra("type", outfitType);
            StartActivityForResult(i, requestCode);
            //StartActivity(i);
        }

        public void RandomOutfit(object sender, EventArgs e)
        {
            // Will be done later, use the Random class with a specified value range to generate the outfit
            // Make this read how many clothes are available first before specifying a value range
        }

        public void CreateOutfitFinal(object sender, EventArgs e)
        {
            string name = Intent.GetStringExtra("name");

            // Ensure first that the required fields such as upper, lower, and feet have values before proceeding
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