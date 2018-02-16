  public class FacebookClient : CookieAwareWebClient
    {
        public bool Login(NameValueCollection a1, string Url)
        {
            var loginResult = this.Login(@"https://www.facebook.com" + Url, a1
        //new NameValueCollection
        //   {
        //   { "email", email },
        //   { "pass", password },
        //    {"login","Login" }
        //    }
        );
            var formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            string file = "1.data";

            using (Stream s = File.Create(file))
                formatter.Serialize(s, this.CookieContainer);

            return loginResult;
        }

        public void GetHomePage()
        {
            // Here's the magic.. Cookies are injected via an overriden
            var webRequest = (HttpWebRequest)this.GetWebRequest(new Uri("https://www.facebook.com/pg/M8tartCool/posts/"));

            string src = "";
            var webResponse = webRequest.GetResponse();

            src = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

            var formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            string file = "1.data";

            using (Stream s = File.Create(file))
                formatter.Serialize(s, webRequest.CookieContainer);

            webResponse.Close();
            System.IO.File.WriteAllText("2.html", System.Net.WebUtility.HtmlDecode(src));


        }
    }

    public class CookieAwareWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; private set; }

        public CookieAwareWebClient()
          : this(new CookieContainer())
        { }

        public CookieAwareWebClient(CookieContainer container)
        {
            CookieContainer = container;
        }


        public bool Login(string loginPageAddress, NameValueCollection loginData)
        {
            var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.89 Safari/537.36 OPR/49.0.2725.39";
            request.Referer = @"https://www.facebook.com";
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
            ///
            var formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            string file = "1.data";

            using (Stream s = File.OpenRead(file))
                request.CookieContainer = (CookieContainer)formatter.Deserialize(s);
            ////
            //  request.CookieContainer = new CookieContainer();

            var response = (HttpWebResponse)request.GetResponse();



            string src = new StreamReader(response.GetResponseStream()).ReadToEnd();

            response.Close();
            System.IO.File.WriteAllText("3.html", System.Net.WebUtility.HtmlDecode(src));

            CookieContainer = request.CookieContainer;
            return response.StatusCode == HttpStatusCode.OK;
        }

        // Add cookies to WebRequest
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);

            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.89 Safari/537.36 OPR/49.0.2725.39";

            request.AllowAutoRedirect = true;
            CookieContainer retrievedCookies = null;


            var formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            string file = "1.data";

            try
            {
                using (Stream s = File.OpenRead(file))
                    retrievedCookies = (CookieContainer)formatter.Deserialize(s);
            }
            catch
            {

                Console.WriteLine("errors cookeis");
                retrievedCookies = new CookieContainer();
            }
            request.CookieContainer = retrievedCookies;
            return request;
        }
    }
