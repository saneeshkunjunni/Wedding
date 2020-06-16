using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wedding.Models;

namespace Wedding.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = GetWishes();
            return View(model);
        }



        public ActionResult FaceBookLogin(String message)
        {
            try
            {



                if (!string.IsNullOrEmpty(message))
                {
                    Session["message"] = message;
                }


                if (Request.Params["error_code"] != null && Request.Params["error_code"] == "200")
                {
                    Response.Redirect("/");
                    return null;
                }

                //if (String.IsNullOrEmpty(InitialPage) == false)
                //{
                //    Response.Cookies.Add(new HttpCookie("InitialPage", InitialPage));
                //}
                //try
                //{


                String uri = "http://" + Request.Url.Authority + Url.Action("FacebookLogin", "Home");



                var FB = new FBOpenGraph("561016054610957", "40ec3611ae4e8b6563436f954ebd6386", uri);

                if (Request.Params["code"] == null)
                {
                    Response.Redirect(FB.GetFacebookLoginUrl());
                    return null;
                }

                FB.SetFacebookToken(Request.Params["code"]);
                var fb = FB.GetOGJson("/me");
                String facebook_id = fb["id"];
                String name = fb["name"];
                string url = FB.GetPicPath("/me/picture?type=large");

                var image = DownloadImage(url);
                if (image != null)
                    SaveImage(image, facebook_id);

                if (Session["message"] != null)
                {
                    var messagetosave = Session["message"].ToString();
                    SaveWish(facebook_id, name, messagetosave);
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
            return null;

        }
        private List<Wishes> GetWishes()
        {

            var context = new EntityContext();
            return context.Wishes.Where(x => x.siteId == 1).ToList(); 
        }
        private void SaveWish(string fbId, string name, string message)
        {

            var wish = new Wishes()
            {
                Name = name,
                fbid = fbId,
                message = message,
                siteId = 1,
            };

            using (var context = new EntityContext())
            {
                context.Wishes.Add(wish);
                context.SaveChanges();
            }
        }

        private Image DownloadImage(string url)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
                return image;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        private void SaveImage(Image image, string fbId)
        {
            var filePath = Server.MapPath("~/Content/images/friends/");
            image.Save(filePath + fbId + ".jpg");
        }
    }
}