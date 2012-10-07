using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Linq;
using System.Json;
using System.IO;
using System.Text;
using System.Web;
using MonoTouch.Foundation;
using MonoTouch.AVFoundation;

namespace BingImageGrid
{
    public delegate void SynchronizerDelegate (List<string> results);
    
    public class Bing
    {
        const string AZURE_KEY = "INSERT YOUR KEY HERE";
        static SynchronizerDelegate sync;
        
        public Bing (SynchronizerDelegate sync)
        {
            Bing.sync = sync;
        }
        
        public void ImageSearch ()
        {
            var t = new Thread (DoSearch);
            t.Start ();
        }
        
        void DoSearch ()
        {
            string uri = "https://api.datamarket.azure.com/Data.ashx/Bing/Search/v1/Image?Query=%27xamarin%27&$top=50&$format=Json";
           
            var httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (uri));
            
            httpReq.Credentials = new NetworkCredential (AZURE_KEY, AZURE_KEY);
            
            try {
                using (HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse ()) {
                    
                    var response = httpRes.GetResponseStream ();
                    var json = (JsonObject)JsonObject.Load (response);
                    
                    var results = (from result in (JsonArray)json ["d"] ["results"]
                                   let jResult = result as JsonObject 
                                   select jResult ["Thumbnail"] ["MediaUrl"].ToString ()).ToList ();

                    if (sync != null)
                        sync (results);
                }
            } catch (Exception) {
                if (sync != null)
                    sync (null);
            }
        }
       
    }
}