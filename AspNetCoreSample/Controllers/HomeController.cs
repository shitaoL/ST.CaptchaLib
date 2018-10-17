using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreSample.Models;
using ST.CaptchaLib;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreSample.Controllers
{
    public class HomeController : Controller
    {
        private const string _cookieCaptchaKey="cookie.captcha.key";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashCaptcha = string.Empty;
                HttpContext.Request.Cookies.TryGetValue(_cookieCaptchaKey, out hashCaptcha);
                if (!CaptchaUtil.VerifyCaptcha(model.Captcha, hashCaptcha))
                {
                    //the captach is not correct
                }
            }
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public IActionResult Captcha()
        {
            try
            {
                string captcha = CaptchaUtil.GetCaptcha(4);
                byte[] _img = CaptchaUtil.GetCaptchaImageBytes(captcha, 100, 30);
                HttpContext.Response.Cookies.Append(_cookieCaptchaKey, CaptchaUtil.HashCaptcha(captcha), new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(1)
                });
                return File(_img, @"image/jpeg");
            }
            catch (Exception ex)
            {
                //write log...
            }
            return null;
        }
    }
}
