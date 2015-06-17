using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using MainBit.Captcha.Services;
using Orchard;
using Orchard.ContentManagement;
using MainBit.Captcha.Models;
using MainBit.Captcha.ViewModel;

namespace MainBit.Captcha.Controllers
{
    public class CaptchaImageController : Controller
    {
        private static int _foregroundArgb = Color.ForestGreen.ToArgb();
        private static Brush _foreground = new SolidBrush(Color.FromArgb(_foregroundArgb));

        private static Color _background = Color.WhiteSmoke;
        private static int _backgroundArgb = _background.ToArgb();

        private readonly static object _padLock = new object();
        private readonly ICaptchaService _captchaService;
        private readonly IOrchardServices _orchardServices;

        public CaptchaImageController(
            ICaptchaService captchaService,
            IOrchardServices orchardServices)
        {
            _captchaService = captchaService;
            _orchardServices = orchardServices;
        }

        public void Render(string challengeGuid, int width, int height)
        {
            // Retrieve the solution text from Session[]
            var captcha = HttpContext.Session[CaptchaServiceConstants.SESSION_KEY_PREFIX + challengeGuid] as CaptchaViewModel;
            if (captcha != null)
            {
                //var settings = _orchardServices.WorkContext.CurrentSite.As<CaptchaSettingsPart>();
                var settings = new CaptchaSettingsPart();
                settings.ImageWidth = width;
                settings.ImageHeight = height;

                if (settings.Foreground != _foregroundArgb)
                {
                    lock (_padLock)
                    {
                        if (settings.Foreground != _foregroundArgb)
                        {
                            _foregroundArgb = settings.Foreground;
                            _foreground.Dispose();
                            _foreground = new SolidBrush(Color.FromArgb(_foregroundArgb));
                        }
                    }
                }

                if (settings.Background != _backgroundArgb)
                {
                    _backgroundArgb = settings.Background;
                    _background = Color.FromArgb(_backgroundArgb);
                }

                // Make a blank canvas to render the CAPTCHA on
                using (Bitmap bmp = new Bitmap(settings.ImageWidth, settings.ImageHeight))
                using (Graphics g = Graphics.FromImage(bmp))
                using (Font font = new Font(settings.ImageFont, 1f))
                {
                    g.Clear(_background);
                    // Perform trial rendering to determine best font size
                    SizeF finalSize;
                    SizeF testSize = g.MeasureString(captcha.Value, font);
                    float bestFontSize = Math.Min(settings.ImageWidth / testSize.Width,
                        settings.ImageHeight / testSize.Height) * 0.95f;
                    using (Font finalFont = new Font(settings.ImageFont, bestFontSize))
                    {
                        finalSize = g.MeasureString(captcha.Value, finalFont);
                    }
                    // Get a path representing the text centered on the canvas
                    g.PageUnit = GraphicsUnit.Point;
                    PointF textTopLeft = new PointF((settings.ImageWidth - finalSize.Width) / 2,
                        (settings.ImageHeight - finalSize.Height) / 2);
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddString(captcha.Value, new FontFamily(settings.ImageFont), 0,
                            bestFontSize, textTopLeft, StringFormat.GenericDefault);
                        // Render the path to the bitmap
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.FillPath(_foreground, DeformPath(path, settings));
                        g.Flush();
                        // Send the image to the response stream in PNG format
                        Response.ContentType = "image/png";
                        using (var memoryStream = new MemoryStream())
                        {
                            bmp.Save(memoryStream, ImageFormat.Png);
                            memoryStream.WriteTo(Response.OutputStream);
                        }
                    }
                }
            }
        }

        private GraphicsPath DeformPath(GraphicsPath path, CaptchaSettingsPart settings)
        {
            Double xFreq = 2 * Math.PI / settings.ImageWidth;
            Double yFreq = 2 * Math.PI / settings.ImageHeight;

            PointF[] deformed = new PointF[path.PathPoints.Length];
            Random rng = new Random();
            Double xSeed = rng.NextDouble() * 2 * Math.PI;
            Double ySeed = rng.NextDouble() * 2 * Math.PI;
            for (int i = 0; i < path.PathPoints.Length; i++)
            {
                PointF original = path.PathPoints[i];
                Double val = xFreq * original.X + yFreq * original.Y;
                int xOffset = (int)(settings.XAmp * Math.Sin(val + xSeed));
                int yOffset = (int)(settings.YAmp * Math.Sin(val + ySeed));
                deformed[i] = new PointF(original.X + xOffset, original.Y + yOffset);
            }
            return new GraphicsPath(deformed, path.PathTypes);
        }
    }
}
