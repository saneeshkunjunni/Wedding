using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace Wedding.Models
{
    public class FBOpenGraph
    {
        public String Appid { get; set; }
        public String AppSecret { get; set; }
        public String MyUrl { get; set; }
        public String Scope { get; set; }
        public String Token { get; set; }

        public FBOpenGraph() { }
        public FBOpenGraph(String Appid, String AppSecret, String MyUrl, String Scope = "email,user_photos")
        {
            this.Appid = Appid;
            this.AppSecret = AppSecret;
            this.MyUrl = MyUrl;
            this.Scope = Scope;
        }

        /// <summary>
        /// After constructing this class, redirect to this address
        /// </summary>
        /// <returns></returns>
        public String GetFacebookLoginUrl()
        {
            return string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}", Appid, HttpUtility.UrlEncode(MyUrl), Scope);
        }

        /// <summary>
        /// Call this Item when 
        /// (Request.Params["code"] != null)
        /// To Set the Token for all Open Graph Calls
        /// </summary>
        /// <param name="Code"></param>
        public void SetFacebookToken(String Code)
        {
            String token_url = String.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}", Appid, HttpUtility.UrlEncode(MyUrl), AppSecret, Code);
            Token = GetUrlContent(token_url).Substring(13).Split('&')[0];
            return;
        }

        /// <summary>
        /// After Authentication, this will return the request objects
        /// for example GetOGJson("/me") will return the current users details
        /// </summary>
        /// <param name="OGPath"></param>
        /// <returns></returns>
        public dynamic GetOGJson(String OGPath)
        {
            String jsonText = GetUrlContent(String.Format("https://graph.facebook.com/{0}?access_token={1}", OGPath.TrimStart('/'), Token));

            // Temporary code for testing only
            try
            {
                // Common.SendMessage("Golf4Less Server", "admin@golf4less.com.au", "Roaming Snaps Errors", "errors@roamingsnaps.com", "Server Error", jsonText, false);
            }
            catch { }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<dynamic>(jsonText);
        }
        public string GetPicPath(String OGPath)
        {
            return  String.Format("https://graph.facebook.com/{0}&access_token={1}", OGPath.TrimStart('/'), Token);
          
                      
        }

        public String GetUrlContent(String Content)
        {
            return new System.Net.WebClient().DownloadString(Content);
        }

        /// <summary>
        /// Useable for downloading a cache of any pictures
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="url"></param>
        public void SaveFromURL(string file_name, string url)
        {
            byte[] content;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(stream))
            {
                content = br.ReadBytes(500000);
                br.Close();
            }
            response.Close();

            FileStream fs = new FileStream(file_name, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(content);
            }
            finally
            {
                fs.Close();
                bw.Close();
            }
        }
    }
}