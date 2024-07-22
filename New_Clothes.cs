using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.Graphics;
using System.IO;
using Android.Provider;
using Android.Media;
using Java.Nio;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using Xamarin.Forms.PlatformConfiguration;
using System.Collections.Specialized;
using System.IO.Pipes;
using System.Collections;
using System.Text.Json;


namespace IT123P___MP
{
    [Activity(Label = "New_Clothes", Theme = "@style/Theme.Design")]
    public class New_Clothes : Activity
    {
        Button back_btn;
        View container;
        HttpWebRequest request;
        HttpWebResponse response;
        int requestCode;
        OutputStream outputstream;
        ImageView iv;
        Android.Graphics.Bitmap bmp;
        Android.Net.Uri imageUri;
        String imageBytesString;
        EditText nameField;
        Spinner typeField;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.new_clothes);

            iv = FindViewById<ImageView>(Resource.Id.imageView);
            nameField = FindViewById<EditText>(Resource.Id.name_edit);
            typeField = FindViewById<Spinner>(Resource.Id.type_spin);
            Button import = FindViewById<Button>(Resource.Id.import_btn);
            Button upload = FindViewById<Button>(Resource.Id.upload_btn);
            
            import.Click += delegate
            {
                Intent i = new Android.Content.Intent(Intent.ActionPick, Android.Provider.MediaStore.Images.Media.ExternalContentUri);
                StartActivityForResult(i, requestCode);
            };
            
            upload.Click += delegate
            {
                if (bmp == null)
                {
                    Toast.MakeText(this, "Please import an image", ToastLength.Short).Show();
                    return;
                }
                else
                {
                    Submit();
                }
            };
            
            FindViewById<Button>(Resource.Id.back_btn).Click += delegate
            {
                Intent i = new Intent(this, typeof(home));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok && data != null)
            {
                imageUri = data.Data;
                bmp = GetBitmap(this.ContentResolver, imageUri);

                Display d = this.WindowManager.DefaultDisplay;
                Android.Util.DisplayMetrics m = new Android.Util.DisplayMetrics();
                d.GetMetrics(m);

                int w = (int)((m.WidthPixels - 8) / 2);
                int h = (int)(120 * m.Density);
                float ratio = (float)h / w;
                int originalw = bmp.Width;
                int originalh = bmp.Height;
                float imageratio = (float)originalh / (float)originalw;
                Android.Graphics.Bitmap bitmap = bmp;
                if (imageratio > ratio)
                {
                    int newh = (int)((float)w / (float)bmp.Width * bmp.Height);
                    int y = Math.Max(0, (newh - h) / 2);
                    bmp = Android.Graphics.Bitmap.CreateScaledBitmap(bmp, w, newh, true);
                    bitmap = Android.Graphics.Bitmap.CreateBitmap(bmp, 0, y, w, h);
                }
                else
                {
                    int neww = (int)((float)h / (float)originalh * originalw);
                    int x = Math.Max(0, (neww - w) / 2);
                    bmp = Android.Graphics.Bitmap.CreateScaledBitmap(bmp, neww, h, true);
                    bitmap = Android.Graphics.Bitmap.CreateBitmap(bmp, x, 0, w, h);
                }

                iv.SetScaleType(ImageView.ScaleType.CenterCrop);
                iv.SetImageBitmap(bitmap);
                
                using (MemoryStream stream = new MemoryStream())
                {
                    bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    stream.Position = 0;
                    byte[] imageBytes = stream.ToArray();
                    imageBytesString = Convert.ToBase64String(imageBytes);
                }
            }
        }
        public unsafe static Bitmap? GetBitmap(ContentResolver? cr, Android.Net.Uri? url)
        {
            JniPeerMembers _members = new XAPeerMembers("android/provider/MediaStore$Images$Media", typeof(Android.Provider.MediaStore.Images.Media));
            try
            {
                JniArgumentValue* ptr = stackalloc JniArgumentValue[2];
                *ptr = new JniArgumentValue(cr?.Handle ?? IntPtr.Zero);
                ptr[1] = new JniArgumentValue(url?.Handle ?? IntPtr.Zero);
                return Java.Lang.Object.GetObject<Bitmap>(_members.StaticMethods.InvokeObjectMethod("getBitmap.(Landroid/content/ContentResolver;Landroid/net/Uri;)Landroid/graphics/Bitmap;", ptr).Handle, JniHandleOwnership.TransferLocalRef);
            }
            finally
            {
                GC.KeepAlive(cr);
                GC.KeepAlive(url);
            }
        }
        public void SaveImage()
        {
            if (imageUri == null)
            { return; }
            string url = $"http://{UtilityClass.ip}/REST/IT123P/MP/API/save_clothe_item.php";

            // Create a WebRequest
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/octet-stream";
            int byteArrayLength = imageBytesString.Length;
            request.ContentLength = byteArrayLength;
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;

            // Write the byte array to the request stream
            using (System.IO.Stream dataStream = request.GetRequestStream())
            {
                byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(imageBytesString);
                dataStream.Write(stringBytes, 0, byteArrayLength);
            }

            // Get the response from the server
            using (WebResponse response = request.GetResponse())
            {
                using (System.IO.Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string responseFromServer = reader.ReadToEnd();
                    }
                }
            }
        }
        public void Submit()
        {
            if (nameField.Length() == 0)
            {
                Toast.MakeText(this, "Please enter a name.", ToastLength.Short).Show();
                return;
            }

            String fileName = nameField.Text;
            String type = typeField.SelectedItem.ToString().ToLower() ;

            SaveImage();

            request = (HttpWebRequest)WebRequest.Create($"http://{UtilityClass.ip}/REST/IT123P/MP/API/change_name.php?name={fileName}&type={type}");
            response = (HttpWebResponse)request.GetResponse();

            Toast.MakeText(this, "New Clothes Added", ToastLength.Long).Show();

            Intent i = new Intent(this, typeof(home));
            i.SetFlags(ActivityFlags.ReorderToFront);
            StartActivity(i);
            Finish();
        }
    }
}