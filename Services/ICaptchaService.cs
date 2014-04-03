using MainBit.Captcha.Models;
using Orchard;
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
        string GenerateCaptcha(string challengeGuid, CaptchaSettingsPart settings = null);
        bool IsCaptchaValid(
            string guidFieldName = CaptchaServiceConstants.CAPTCHA_DEFAULT_GUID_FIELD_NAME,
            string valueFieldName = CaptchaServiceConstants.CAPTCHA_DEFAULT_VALUE_FIELD_NAME);
    }
}