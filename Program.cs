using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            CookieContainer gaCookies = new CookieContainer();
            Uri target = new Uri("https://www.facebook.com/M8tartCool/posts/");

            gaCookies.Add(new Cookie("sb", "zFaYWvAncQYxwpo2YHyvnhyy") { Domain = target.Host });
            gaCookies.Add(new Cookie("fr", "0vW5WENEgC9XJzqix.AWXaK3WFXvdmzNDHNd1DmSHJJl0.BamFbM.c_.AAA.0.0.BamFbP.AWUIG5Cx") { Domain = target.Host });
            gaCookies.Add(new Cookie("c_user", "100001599066593") { Domain = target.Host });
            gaCookies.Add(new Cookie("xs", "7%3Al4KkXowz_Yf_mA%3A2%3A1519933135%3A18008%3A6878") { Domain = target.Host });
            gaCookies.Add(new Cookie("pl", "n") { Domain = target.Host });

            
            APIPorjectanalistic.ApiPorject m = new APIPorjectanalistic.ApiPorject(gaCookies);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string g =   m.GoPages(target.ToString());

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs.ToString());
            Console.ReadKey();



            ///----------------------------------
            ///

            string[] Files = { };
              NameValueCollection PostCollection = new NameValueCollection();
            PostCollection.Add("Users", "value");
        //   PostCollection.Add("Users1", "value2");
          //  PostCollection.Add("Users2", "value2");
            string  OutPut = APIPorjectanalistic.ApiPorject.UploadFilesToRemoteUrl(target.ToString(), Files, PostCollection);

        }
    }
}
