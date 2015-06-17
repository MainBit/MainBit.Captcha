using MainBit.Captcha.Models;
using MainBit.Captcha.ViewModel;
using Orchard;
using System;
namespace MainBit.Captcha.Services
{
    internal static class CaptchaServiceConstants
    {
        internal const string CAPTCHA_DEFAULT_VALUE_FIELD_NAME = "captchaattempt";
        internal const string CAPTCHA_DEFAULT_GUID_FIELD_NAME = "captchaguid";
        internal const string SESSION_KEY_PREFIX = "__Captcha";
    }

    public interface ICaptchaService : IDependency
    {
        CaptchaSettingsPart GetSettings();
        CaptchaViewModel GetOrGenerateCaptcha(Guid challengeGuid);
        bool IsCaptchaValid(Guid challengeGuid, string value);
    }
}