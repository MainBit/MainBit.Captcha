using System;
using System.Linq;
using System.Web.Mvc;
using MainBit.Captcha.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Mvc;
using Orchard;
using Orchard.ContentManagement;

namespace MainBit.Captcha.Services
{
    public class DefaultCaptchaService : ICaptchaService
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IOrchardServices _services;

        public DefaultCaptchaService(
            IContentDefinitionManager contentDefinitionManager,
            IOrchardServices services)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _services = services;
        }

        public string GenerateCaptcha(
            string challengeGuid,
            CaptchaSettingsPart settings = null)
        {
            if (settings == null)
            {
                //var settings = _services.WorkContext.CurrentSite.As<CaptchaSettingsPart>();
                settings = new CaptchaSettingsPart();
            }
            var httpContext = _services.WorkContext.HttpContext;

            // Generate and store random solution text
            httpContext.Session[CaptchaServiceConstants.SESSION_KEY_PREFIX + challengeGuid] = MakeRandomSolution(settings);

            var urlHelper = new UrlHelper(httpContext.Request.RequestContext);

            return urlHelper.Action("Render", "CaptchaImage",
                new { area = "MainBit.Captcha", challengeGuid, height = settings.ImageHeight, width = settings.ImageWidth });
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

        public bool IsCaptchaValid(
            string guidFieldName = CaptchaServiceConstants.CAPTCHA_DEFAULT_GUID_FIELD_NAME,
            string valueFieldName = CaptchaServiceConstants.CAPTCHA_DEFAULT_VALUE_FIELD_NAME)
        {
            //var settings = _services.WorkContext.CurrentSite.As<CaptchaSettingsPart>();
            var settings = new CaptchaSettingsPart();
            var httpContext = _services.WorkContext.HttpContext;

            if (settings.IsForNotAuthUsersOnly && httpContext.Request.IsAuthenticated)
                return true;

            var challengeGuid = httpContext.Request.Params[guidFieldName];
            var attemptedSolution = httpContext.Request.Params[valueFieldName];
            var guidKey = CaptchaServiceConstants.SESSION_KEY_PREFIX + challengeGuid;

            // Immediately remove the solution from Session to prevent replay attacks
            var solution = (string)httpContext.Session[guidKey];
            httpContext.Session.Remove(guidKey);

            return guidKey != null && solution != null &&
                attemptedSolution.Equals(solution, StringComparison.OrdinalIgnoreCase);
        }
    }
}
