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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
            }
            return View();
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
                HttpContext.Response.Cookies.Append("cookie.captcha.key", captcha.ToUpper(), new CookieOptions //captcha.ToUpper() shdould by encrypted, I omitted here.
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
