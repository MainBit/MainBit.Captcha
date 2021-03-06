﻿using MainBit.Captcha.Models;
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
        public string Src { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Guid Guid { get; set; }
        public string Value { get; set; }
    }
}