using Android.Graphics;
using System.IO;
using System;
using System.Net;
using Android.Util;
using System.Collections.Specialized;

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
        public static void UploadFilesToRemoteUrl(string url, string[] files)
        {

            long length = 0;
            string boundary = "----------------------------" +
            DateTime.Now.Ticks.ToString("x ");
            HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest2.ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest2.Method = "POST";
            httpWebRequest2.KeepAlive = true;

            httpWebRequest2.Credentials =
            System.Net.CredentialCache.DefaultCredentials;

            System.IO.Stream memStream = new System.IO.MemoryStream();

            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            memStream.Write(boundarybytes, 0, boundarybytes.Length);
            length += boundarybytes.Length;

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\";" +
                "filename =\"{1}\"\r\n Content-Type: application / octet - stream\r\n\r\n";
            for (int i = 0; i < files.Length; i++)
            {

                string header = string.Format(headerTemplate, " file" + i, files[i]);

                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                memStream.Write(headerbytes, 0, headerbytes.Length);
                length += headerbytes.Length;

                FileStream fileStream = new FileStream(files[i], FileMode.Open,
                FileAccess.Read);
                byte[] buffer = new byte[1024];

                int bytesRead = 0;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                    length += bytesRead;
                }
                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                length += boundarybytes.Length;

                fileStream.Close();
            }

            httpWebRequest2.ContentLength = memStream.Length;

            System.IO.Stream requestStream = httpWebRequest2.GetRequestStream();

            memStream.Position = 0;
            byte[] tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();
            WebResponse webResponse2 = httpWebRequest2.GetResponse();

            //System.IO.Stream stream2 = webResponse2.GetResponseStream();
            //StreamReader reader2 = new StreamReader(stream2);
            //Toast.MakeText(this,reader2.ReadToEnd(),ToastLength.Long).Show();

            webResponse2.Close();
            httpWebRequest2 = null;
            webResponse2 = null;

        }
        public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }
        public static void UploadImage(string url, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

        }
        public static void AnotherOne()
        {
            //HttpWebRequest POSTreq =
            //(HttpWebRequest)WebRequest.Create("http://82.41.255.140/api/post-ptr");

            //string POSTdata = "action=" + HttpUtility.UrlEncode("date");
            //byte[] data = Encoding.ASCII.GetBytes(POSTdata);

            //POSTreq.Method = "POST";
            //POSTreq.ContentType = "application/x-www-form-urlencoded";
            //POSTreq.ContentLength = data.LongLength;

            //POSTreq.GetRequestStream().Write(data, 0, data.Length);
        }
    }
}