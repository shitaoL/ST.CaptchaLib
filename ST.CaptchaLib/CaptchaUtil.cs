using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ST.CaptchaLib
{
    public static class CaptchaUtil
    {
        private const string _randomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        private const int _length = 5;

        /// <summary>
        /// Generate captcha based on length
        /// </summary>
        /// <param name="length">captcha length</param>
        /// <returns></returns>
        public static string GetCaptcha(int length = _length)
        {
            return GetCaptcha(_randomChars, length);
        }

        /// <summary>
        /// Generate captcha based on random chars and length
        /// </summary>
        /// <param name="randomChars">random chars, for example:ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890</param>
        /// <param name="length">captcha length</param>
        /// <returns></returns>

        public static string GetCaptcha(string randomChars, int length = _length)
        {
            char[] captcha = new char[length];
            char[] dic = randomChars.ToArray();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(dic.Length - 1);
                captcha[i] = dic[index];
            }
            return new string(captcha);
        }

        /// <summary>
        /// Get captcha image bytes
        /// </summary>
        /// <param name="captcha">captcha string</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <returns></returns>
        public static byte[] GetCaptchaImageBytes(string captcha, int width = 120, int height = 30)
        {
            Font font = new Font("Arial", 14, FontStyle.Bold);
            Bitmap bitmap = new Bitmap(width, height);
            Graphics gph = Graphics.FromImage(bitmap);
            SizeF totalSizeF = gph.MeasureString(captcha, font);
            float startX = (width - totalSizeF.Width - (totalSizeF.Width / (captcha.Length))) / 2;
            float startY = (height - totalSizeF.Height) / 2;
            PointF startPointF = new PointF(startX, startY);
            gph.Clear(Color.White);
            Brush brush = null;
            SizeF curCharSizeF;
            Random random = new Random();
            for (int i = 0; i < captcha.Length; i++)
            {
                brush = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)), Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
                gph.DrawString(captcha[i].ToString(), font, brush, startPointF);
                curCharSizeF = gph.MeasureString(captcha[i].ToString(), font);
                startPointF.X += curCharSizeF.Width;
            }
            //画图片的干扰线
            for (int i = 0; i < 5; i++)
            {
                int x1 = random.Next(bitmap.Width);
                int x2 = random.Next(bitmap.Width);
                int y1 = random.Next(bitmap.Height);
                int y2 = random.Next(bitmap.Height);
                gph.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            //画图片的前景干扰点
            for (int i = 0; i < 30; i++)
            {
                int x = random.Next(bitmap.Width);
                int y = random.Next(bitmap.Height);
                bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            gph.Dispose();
            return stream.ToArray();
        }

    }
}
