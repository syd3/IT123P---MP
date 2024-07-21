using Android.Graphics;
using System.Net;

namespace IT123P___MP
{
    public static class UtilityClass
    {
        public static string ip = "192.168.1.14";
        
        // Call this method class via: var imgBp = UtilityClass.GetImageBitmapFromUrl(URL); imageButtonVarName = SetImageBitmap(imgBp);
        public static Bitmap GetImageBitmapFromUrl(string url) // To retrieve an image from a URL
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                try
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                } catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var httpResponse = ex.Response as HttpWebResponse;
                        if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.NotFound)
                        {
                            return null;
                        }
                    }
                }
            }
            return imageBitmap;
        }
    }
}