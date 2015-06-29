using System;
using System.Linq;
using System.Web.Mvc;
using MainBit.Captcha.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Mvc;
using Orchard;
using Orchard.ContentManagement;
using MainBit.Captcha.ViewModel;

namespace MainBit.Captcha.Services
{
    public class DefaultCaptchaService : ICaptchaService
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IOrchardServices _services;
        private readonly UrlHelper _urlHelper;

        public DefaultCaptchaService(
            IContentDefinitionManager contentDefinitionManager,
            IOrchardServices services,
            UrlHelper urlHelper)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _services = services;
            _urlHelper = urlHelper;
        }

        public CaptchaSettingsPart GetSettings()
        {
            return new CaptchaSettingsPart();
        }

        public CaptchaViewModel GetOrGenerateCaptcha(Guid challengeGuid)
        {
            var captcha = _services.WorkContext.HttpContext.Session[CaptchaServiceConstants.SESSION_KEY_PREFIX + challengeGuid] as CaptchaViewModel;
            if(captcha == null) {
                var settings = GetSettings();

                captcha = new CaptchaViewModel()
                {
                    Guid = challengeGuid,
                    Src = _urlHelper.Action("GetCaptchaImage", "CaptchaImage",
                        new { area = "MainBit.Captcha", challengeGuid, height = settings.ImageHeight, width = settings.ImageWidth }),
                    Width = settings.ImageWidth,
                    Height = settings.ImageHeight,
                    Value = MakeRandomSolution(settings)
                };

                _services.WorkContext.HttpContext.Session[CaptchaServiceConstants.SESSION_KEY_PREFIX + challengeGuid] = captcha;
            }

            return captcha;
        }


        private static string MakeRandomSolution(CaptchaSettingsPart settings)
        {
            Random rng = new Random();

            char[] buf = new char[settings.TotalChars];
            for (int i = 0; i < settings.TotalChars; i++)
            {
                Func<char> RndDigit = () => (char)('0' + rng.Next(10));
                Func<char> RndLetter = () => (char)('a' + rng.Next(26));

                if (settings.IncDigits && settings.IncLetters)
                    buf[i] = rng.Next(11) > 5 ? RndDigit() : RndLetter();
                else if (settings.IncDigits)
                    buf[i] = RndDigit();
                else
                    buf[i] = RndLetter();
            }

            return new string(buf);
        }

        public bool IsCaptchaValid(Guid challengeGuid, string value)
        {
            var captcha = _services.WorkContext.HttpContext.Session[CaptchaServiceConstants.SESSION_KEY_PREFIX + challengeGuid] as CaptchaViewModel;
            if(captcha == null) { return false; }

            return string.Equals(captcha.Value, value, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
