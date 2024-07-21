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
    [Activity(Label = "New_Clothes")]
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
        EditText typeField;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.new_clothes);

            iv = FindViewById<ImageView>(Resource.Id.imageView);
            nameField = FindViewById<EditText>(Resource.Id.name_edit);
            typeField = FindViewById<EditText>(Resource.Id.type_spin);
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
                { return; }
                else
                {
                    Submit();
                }
            };
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok && data != null)
            {
                imageUri = data.Data;
                bmp = GetBitmap(this.ContentResolver, imageUri);
                iv.SetImageBitmap(bmp);
                using (MemoryStream stream = new MemoryStream())
                {
                    bmp.Compress(Bitmap.CompressFormat.Png, 100, stream);
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
                        FindViewById<TextView>(Resource.Id.debug_text).Text = responseFromServer;
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
            String type = typeField.Text;

            SaveImage();

            request = (HttpWebRequest)WebRequest.Create($"http://{UtilityClass.ip}/REST/IT123P/MP/API/change_name.php?name={fileName}&type={type}");
            response = (HttpWebResponse)request.GetResponse(); // Web Request to retrieve the file name of the images
        }
    }
}