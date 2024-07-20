using System;
using System.Net;
using System.IO;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace IT123P___MP
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText edit1, edit2;
        Button btn;
        string username, password, res;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);

            edit1 = FindViewById<EditText>(Resource.Id.editText1);
            edit2 = FindViewById<EditText>(Resource.Id.editText2);
            btn = FindViewById<Button>(Resource.Id.button1);

            btn.Click += this.Login;
        }

        public void Login(object sender, EventArgs e)
        {
            username = edit1.Text;
            password = edit2.Text;

            string local_ip = UtilityClass.ip;
            request = (HttpWebRequest)WebRequest.Create($"http://{local_ip}/REST/IT123P/MP/API/user_login.php?uname={username}&pword={password}");
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();
            Toast.MakeText(this, res, ToastLength.Long).Show();

            if (res.Contains("Ok!"))
            {
                Intent i = new Intent(this, typeof(home));
                i.PutExtra("name", username);
                StartActivity(i);
                Finish();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}