using System;
using MainBit.Captcha.Models;
using MainBit.Captcha.Services;
using Orchard.ContentManagement.Drivers;
using Orchard.Mvc;
using Orchard.ContentManagement;
using MainBit.Captcha.ViewModel;
using Orchard.Localization;

namespace MainBit.Captcha.Drivers
{
    public class CaptchaPartDriver : ContentPartDriver<CaptchaPart>
    {
        private readonly ICaptchaService _captchaService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CaptchaPartDriver(ICaptchaService captchaService, IHttpContextAccessor httpContextAccessor)
        {
            _captchaService = captchaService;
            _httpContextAccessor = httpContextAccessor;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "Captcha"; } }

        //protected override DriverResult Display(CaptchaPart part, string displayType, dynamic shapeHelper)
        //{
            //return ContentShape("Parts_Captcha", () =>
            //{
            //    var captchaVM = new CaptchaViewModel();
            //    captchaVM.Src = _captchaService.GenerateCaptcha(captchaVM.Guid.ToString());
            //    var result = shapeHelper.Parts_Captcha(CaptchaVM: captchaVM);
            //    return result;
            //});
        //}

        protected override DriverResult Editor(CaptchaPart part, dynamic shapeHelper)
        {
            //var settings = _services.WorkContext.CurrentSite.As<CaptchaSettingsPart>();
            var settings = new CaptchaSettingsPart();
            var httpContext = _httpContextAccessor.Current();

            if (settings.IsForNotAuthUsersOnly && httpContext.Request.IsAuthenticated)
                return null;

            var captchaVM = new CaptchaViewModel();
            captchaVM.Src = _captchaService.GenerateCaptcha(captchaVM.Guid.ToString());
            return ContentShape("Parts_Captcha_Edit", () => shapeHelper.EditorTemplate(TemplateName: "Parts/Captcha", Model: captchaVM, Prefix: Prefix));
            //return Combined(                
            //);
        }

        protected override DriverResult Editor(CaptchaPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var captchaVM = new CaptchaViewModel();
            //updater.TryUpdateModel(part, Prefix, null, null);
            if (!_captchaService.IsCaptchaValid())
            {
                updater.AddModelError("Captcha", T("Captcha invalid."));
            }
            return Editor(part, shapeHelper);
        }
    }
}
