using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using Java.Security.Cert;
using System.Linq;
using Java.Lang;
using Android.Provider;
using Android.Runtime;
using Java.Interop;
using Android.Graphics;
using static Android.Icu.Text.CaseMap;

namespace IT123P___MP
{
    [Activity(Label = "ViewOutfit")]
    public class ViewOutfit : Activity
    {
        Button button1, button2, button3, button4, back;
        TextView textView;
        List<string> outfitNames;
        string name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_outfits);

            button1 = FindViewById<Button>(Resource.Id.button1);
            button2 = FindViewById<Button>(Resource.Id.button2);
            button3 = FindViewById<Button>(Resource.Id.button3);
            button4 = FindViewById<Button>(Resource.Id.button4);
            back = FindViewById<Button>(Resource.Id.back_btn);
            textView = FindViewById<TextView>(Resource.Id.textView1);

            button1.Click += async (sender, e) => await DisplayOutfitNames(0);
            button2.Click += async (sender, e) => await DisplayOutfitNames(1);
            button3.Click += async (sender, e) => await DisplayOutfitNames(2);
            button4.Click += async (sender, e) => await DisplayOutfitNames(3);

            back.Click += delegate
            {
                Intent i = new Intent(this, typeof(home));
                i.SetFlags(ActivityFlags.ReorderToFront);
                StartActivity(i);
                Finish();
            };

            FetchOutfitNames();
        }
        

        private async void FetchOutfitNames()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            using (HttpClient client = new HttpClient(handler))
            {
                string url = "https://192.168.100.63/REST/IT123P/MP/API/fetch_outfit.php";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    //string content = await response.Content.ReadAsStringAsync();
                    ////Toast.MakeText(this, content, ToastLength.Long).Show();
                    //JsonDocument jason = JsonDocument.Parse(content);
                    //JsonElement root = jason.RootElement;
                    //for(int i = 0; i < root.GetArrayLength(); i++)
                    //{
                    //    outfitNames.Add(root[i].ToString());
                    //}
                    //UpdateButtons();
                }
                else
                {

                    Toast.MakeText(this, "Failed to fetch outfit names", ToastLength.Short).Show();
                }
            }
        }

        private void UpdateButtons()
        {
            if (outfitNames != null && outfitNames.Count >= 4)
            {
                button1.Text = outfitNames[0];
                button2.Text = outfitNames[1];
                button3.Text = outfitNames[2];
                button4.Text = outfitNames[3];
            }
        }

        private async Task DisplayOutfitNames(int index)
        {
            if (outfitNames != null && index < outfitNames.Count)
            {

                textView.Text = outfitNames[index];
            }
        }
        
    }
}
