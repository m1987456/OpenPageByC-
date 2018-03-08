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

            gaCookies.Add(new Cookie("sb", "exm") { Domain = target.Host });


            
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
