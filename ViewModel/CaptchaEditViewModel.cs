using MainBit.Captcha.Models;
using MainBit.Captcha.Services;
using Orchard.Mvc.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MainBit.Captcha.ViewModel
{
    public class CaptchaEditViewModel
    {
        public CaptchaViewModel Captcha { get; set; }

        [DisplayName("Captcha")]
        [Required]
        public string Value { get; set; }
    }
}