using MainBit.Captcha.Models;
using MainBit.Captcha.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MainBit.Captcha.ViewModel
{
    public class CaptchaViewModel
    {
        public CaptchaViewModel()
        {
            GuidFieldName = CaptchaServiceConstants.CAPTCHA_DEFAULT_GUID_FIELD_NAME;
            ValueFieldName = CaptchaServiceConstants.CAPTCHA_DEFAULT_VALUE_FIELD_NAME;
            Guid = Guid.NewGuid();
        }
        public string Src { get; set; }
        public Guid Guid { get; set; }
        public string GuidFieldName { get; set; }
        //[Required]
        public string ValueFieldName { get; set; }
    }
}