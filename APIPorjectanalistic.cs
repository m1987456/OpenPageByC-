using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace APIPorjectanalistic
{
    public class ApiPorject : CookieAwareWebClient
    {
       public  ApiPorject(CookieContainer container) : base(container)
        {
            
        }
        public string Post(NameValueCollection a1, string Url)
        {
            var loginResult = this.SendPost(Url, a1
        //new NameValueCollection
        //   {
        //   { "email", email },
        //   { "pass", password },
        //    {"login","Login" }
        //    }
        );


            return loginResult;
        }

        public string GoPages(string URl)
        {
            // Here's the magic.. Cookies are injected via an overriden
            var webRequest = (HttpWebRequest)this.GetWebRequest(new Uri(URl));
            
            string src = "";
            var webResponse = webRequest.GetResponse();

            src = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();


            return src;


        }

        public void Upload(string uri, string filePath)
        {
            string formdataTemplate = "Content-Disposition: form-data; filename=\"{0}\";\r\nContent-Type: image/jpeg\r\n\r\n";
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ServicePoint.Expect100Continue = false;
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + boundary;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, Path.GetFileName(filePath));
                    byte[] formbytes = Encoding.UTF8.GetBytes(formitem);
                    requestStream.Write(formbytes, 0, formbytes.Length);
                    byte[] buffer = new byte[1024 * 4];
                    int bytesLeft = 0;

                    while ((bytesLeft = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        requestStream.Write(buffer, 0, bytesLeft);
                    }

                }
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) { }

                Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public static string UploadFilesToRemoteUrl(string url, string[] files, NameValueCollection formFields = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" +
                                    boundary;
            request.Method = "POST";
            request.KeepAlive = true;

            Stream memStream = new System.IO.MemoryStream();

            var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                    boundary + "\r\n");
            var endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                        boundary + "--");


            string formdataTemplate = "\r\n--" + boundary +
                                        "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            if (formFields != null)
            {
                foreach (string key in formFields.Keys)
                {
                    string formitem = string.Format(formdataTemplate, key, formFields[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }
            }

            string headerTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                "Content-Type: application/octet-stream\r\n\r\n";


            for (int i = 0; i < files.Length; i++)
            {
                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                var header = string.Format(headerTemplate, "uplTheFile" + i.ToString(), files[i]);
                var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                memStream.Write(headerbytes, 0, headerbytes.Length);

                using (var fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[1024];
                    var bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        memStream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            request.ContentLength = memStream.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            }

            using (var response = request.GetResponse())
            {
                Stream stream2 = response.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                return reader2.ReadToEnd();
            }
        }

        public static string UploadFilesToRemoteUrl(string url, List<byte[]> files, NameValueCollection formFields = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" +
                                    boundary;
            request.Method = "POST";
            request.KeepAlive = true;

            Stream memStream = new System.IO.MemoryStream();

            var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                    boundary + "\r\n");
            var endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                        boundary + "--");


            string formdataTemplate = "\r\n--" + boundary +
                                        "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            if (formFields != null)
            {
                foreach (string key in formFields.Keys)
                {
                    string formitem = string.Format(formdataTemplate, key, formFields[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }
            }

            string headerTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                "Content-Type: application/octet-stream\r\n\r\n";


            for (int i = 0; i < files.Count; i++)
            {
                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                var header = string.Format(headerTemplate, "uplTheFile" + i.ToString(), "image" + i.ToString() + ".jpg");
                var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                memStream.Write(headerbytes, 0, headerbytes.Length);
                memStream.Write(files[i], 0, files[i].Length);


            }

            memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            request.ContentLength = memStream.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            }

            using (var response = request.GetResponse())
            {
                Stream stream2 = response.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                return reader2.ReadToEnd();
            }
        }
    }

    public class CookieAwareWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; private set; }

        public CookieAwareWebClient()
          : this(new CookieContainer())
        { CookieContainer = new CookieContainer(); }

        public CookieAwareWebClient(CookieContainer container)
        {
            CookieContainer = container;
        }


        public string SendPost(string loginPageAddress, NameValueCollection loginData)
        {
            var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);
            //   request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //  request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.89 Safari/537.36 OPR/49.0.2725.39";
            //     request.Referer = @"https://www.facebook.com";
            request.AllowAutoRedirect = true;
            var parameters = new StringBuilder();
            foreach (string key in loginData.Keys)
            {
                parameters.AppendFormat("{0}={1}&",
                    HttpUtility.UrlEncode(key),
                    HttpUtility.UrlEncode(loginData[key]));
            }
            parameters.Length -= 1;

            var buffer = Encoding.ASCII.GetBytes(parameters.ToString());
            request.ContentLength = buffer.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();

            request.CookieContainer = this.CookieContainer;

            var response = (HttpWebResponse)request.GetResponse();



            string src = new StreamReader(response.GetResponseStream()).ReadToEnd();


            this.CookieContainer = request.CookieContainer;
            return src;
        }

        // Add cookies to WebRequest
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);

            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";


            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.89 Safari/537.36 OPR/49.0.2725.39";

            request.AllowAutoRedirect = true;

            request.CookieContainer = this.CookieContainer;

            return request;
        }

    }
}
