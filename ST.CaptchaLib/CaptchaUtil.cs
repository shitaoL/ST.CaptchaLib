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
        public static string GetCaptcha(int length = 5)
        {
            char[] _captcha = new char[length];
            string _code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            char[] _dic = _code.ToArray();
            Random _random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = _random.Next(_dic.Length - 1);
                _captcha[i] = _dic[index];
            }
            return new string(_captcha);
        }

        public static byte[] GetCaptchaImageBytes(string captcha, int width = 120, int height = 30)
        {
            Font _font = new Font("Arial", 14, FontStyle.Bold);            
            Bitmap _bitmap = new Bitmap(width, height);
            Graphics _gh = Graphics.FromImage(_bitmap);
            SizeF _totalSizeF = _gh.MeasureString(captcha, _font);            
            float _startX = (width - _totalSizeF.Width - (_totalSizeF.Width / (captcha.Length))) / 2;
            float _startY = (height - _totalSizeF.Height) / 2;
            PointF _startPointF = new PointF(_startX, _startY);
            _gh.Clear(Color.White);            
            Brush _brush = null;
            SizeF _curCharSizeF;
            Random _random = new Random();
            for (int i = 0; i < captcha.Length; i++)
            {
                _brush = new LinearGradientBrush(new Point(0, 0), new Point(1, 1), Color.FromArgb(_random.Next(255), _random.Next(255), _random.Next(255)), Color.FromArgb(_random.Next(255), _random.Next(255), _random.Next(255)));
                _gh.DrawString(captcha[i].ToString(), _font, _brush, _startPointF);
                _curCharSizeF = _gh.MeasureString(captcha[i].ToString(), _font);
                _startPointF.X += _curCharSizeF.Width;
            }
            //画图片的干扰线
            for (int i = 0; i < 5; i++)
            {
                int x1 = _random.Next(_bitmap.Width);
                int x2 = _random.Next(_bitmap.Width);
                int y1 = _random.Next(_bitmap.Height);
                int y2 = _random.Next(_bitmap.Height);
                _gh.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            //画图片的前景干扰点
            for (int i = 0; i < 30; i++)
            {
                int x = _random.Next(_bitmap.Width);
                int y = _random.Next(_bitmap.Height);
                _bitmap.SetPixel(x, y, Color.FromArgb(_random.Next()));
            }
            MemoryStream stream = new MemoryStream();
            _bitmap.Save(stream, ImageFormat.Png);
            _gh.Dispose();
            return stream.ToArray();
        }

    }
}
