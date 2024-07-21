using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace IT123P___MP
{
    [Activity(Label = "register")]
    public class register : Activity
    {
        EditText edit1, edit2, edit3;
        Button btn1, btn2;
        ImageView logo;
        string username, password, confPassword, res;

        HttpWebRequest request;
        HttpWebResponse response;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register);

            logo = FindViewById<ImageView>(Resource.Id.imageView1);
            edit1 = FindViewById<EditText>(Resource.Id.editText1);
            edit2 = FindViewById<EditText>(Resource.Id.editText2);
            edit3 = FindViewById<EditText>(Resource.Id.editText3);
            btn1 = FindViewById<Button>(Resource.Id.button1);
            btn2 = FindViewById<Button>(Resource.Id.button2);

            logo.SetImageResource(Resource.Drawable.logo);

            btn1.Click += Register;

            btn2.Click += delegate
            {
                Intent i = new Intent(this, typeof(MainActivity));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };
        }

        public void Register(object sender, EventArgs e)
        {
            username = edit1.Text;
            password = edit2.Text;
            confPassword = edit3.Text;

            if (password != confPassword)
            {
                Toast.MakeText(this, "Passwords don't match", ToastLength.Long).Show();
                edit2.Text = "";
                edit3.Text = "";
            } else
            {
                string local_ip = UtilityClass.ip;
                request = (HttpWebRequest)WebRequest.Create($"http://{local_ip}/REST/IT123P/MP/API/user_register.php?uname={username}&pword={password}");
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                res = reader.ReadToEnd();
                Toast.MakeText(this, res, ToastLength.Long).Show();

                if (res.Contains("User Registered"))
                {
                    Intent i = new Intent(this, typeof(home));
                    i.PutExtra("name", username);
                    StartActivity(i);
                    Finish();
                }
            }
        }
    }
}