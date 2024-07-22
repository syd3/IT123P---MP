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
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Design", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText edit1, edit2;
        Button btn, btn2;
        ImageView logo;
        string username, password, res;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);

            logo = FindViewById<ImageView>(Resource.Id.imageView1);
            edit1 = FindViewById<EditText>(Resource.Id.editText1);
            edit2 = FindViewById<EditText>(Resource.Id.editText2);
            btn = FindViewById<Button>(Resource.Id.button1);
            btn2 = FindViewById<Button>(Resource.Id.button2);

            logo.SetImageResource(Resource.Drawable.logo);

            btn.Click += this.Login;

            btn2.Click += delegate
            {
                Intent i = new Intent(this, typeof(register));
                StartActivity(i);
                Finish();
            };
        }

        public void Login(object sender, EventArgs e)
        {
            username = edit1.Text;
            password = edit2.Text;

            if (username == "" || password == "")
            {
                Toast.MakeText(this, "Please fill out the Username and Password fields", ToastLength.Short).Show();
            } else
            {
                string local_ip = UtilityClass.ip;
                request = (HttpWebRequest)WebRequest.Create($"http://{local_ip}/REST/IT123P/MP/API/user_login.php?uname={username}&pword={password}");
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                res = reader.ReadToEnd();
                Toast.MakeText(this, res, ToastLength.Long).Show();

                if (res.Contains("Logged In"))
                {
                    Intent i = new Intent(this, typeof(home));
                    i.PutExtra("name", username);
                    StartActivity(i);
                    Finish();
                }

                reader.Dispose();
                response.Close();
                response.Dispose();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}