using Android.Graphics;
using System.Net;

namespace IT123P___MP
{
    public static class UtilityClass
    {
        // Call this method class via: var imgBp = UtilityClass.GetImageBitmapFromUrl(URL); imageButtonVarName = SetImageBitmap(imgBp);
        public static Bitmap GetImageBitmapFromUrl(string url) // To retrieve an image from a URL
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}