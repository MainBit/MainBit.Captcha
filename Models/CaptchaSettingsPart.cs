using Orchard.ContentManagement;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace MainBit.Captcha.Models {

    public class CaptchaSettingsPart
    {
        private int? _foreground;
        private int? _background;
        private int? _totalChars;
        private int? _imageWidth;
        private int? _imageHeight;
        private double? _xAmp;
        private double? _yAmp;

        private bool? _incLetters;
        private bool? _incDigits;
        private bool? _isForNotAuthUsersOnly;

        private string _imageFont;

        public bool IsForNotAuthUsersOnly
        {
            get
            {
                if (!_isForNotAuthUsersOnly.HasValue)
                    _isForNotAuthUsersOnly = true;

                return _isForNotAuthUsersOnly.Value;
            }
            set { _isForNotAuthUsersOnly = value; }
        }

        public bool IncLetters
        {
            get
            {
                if (!_incLetters.HasValue)
                    _incLetters = false;

                return _incLetters.Value;
            }
            set { _incLetters = value; }
        }

        public bool IncDigits
        {
            get
            {
                if (!_incDigits.HasValue)
                    _incDigits = true;

                return _incDigits.Value;
            }
            set { _incDigits = value; }
        }

        [Range(0.0, 10.0)]
        public double YAmp
        {
            get
            {
                if (!_yAmp.HasValue)
                    _yAmp = 3.5;

                return _yAmp.Value;
            }
            set { _yAmp = value; }
        }

        [Range(0.0, 10.0)]
        public double XAmp
        {
            get
            {
                if (!_xAmp.HasValue)
                    _xAmp = 6.5;

                return _xAmp.Value;
            }
            set { _xAmp = value; }
        }

        [StringLength(100)]
        public string ImageFont
        {
            get
            {
                if (_imageFont == null)
                    _imageFont = "Arial";

                return _imageFont;
            }
            set { _imageFont = value; }
        }

        [Range(1, 1024)]
        public int ImageHeight
        {
            get
            {
                if (!_imageHeight.HasValue)
                    _imageHeight = 50;

                return _imageHeight.Value;
            }
            set { _imageHeight = value; }
        }

        [Range(1, 1024)]
        public int ImageWidth
        {
            get
            {
                if (!_imageWidth.HasValue)
                    _imageWidth = 150;

                return _imageWidth.Value;
            }
            set { _imageWidth = value; }
        }

        [Range(1, 10)]
        public int TotalChars
        {
            get
            {
                if (!_totalChars.HasValue)
                    _totalChars = 5;

                return _totalChars.Value;
            }
            set { _totalChars = value; }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0x{0:X}")]
        public int Foreground
        {
            get
            {
                if (!_foreground.HasValue)
                    _foreground = Color.Black.ToArgb();

                return _foreground.Value;
            }
            set { _foreground = value; }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0x{0:X}")]
        public int Background
        {
            get
            {
                if (!_background.HasValue)
                    _background = Color.WhiteSmoke.ToArgb();

                return _background.Value;
            }
            set { _background = value; }
        }
    }
}
