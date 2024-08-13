using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public static class Keys
    {
        public static class Color
        {
            public static readonly string Green_Correct = "#51C994";
            public static readonly string Purple_Selected = "#686dd2";
            public static readonly string Grey = "#E6E6EA";
            public static readonly string CoralBlue = "#6DD4D6";
            public static readonly string Red = "#e6356b";
            public static readonly string Black_Text = "#272727";
            public static readonly string Purple_UI = "#7D62FF";
            public static readonly string Grey_Placeholder = "#32323280";
        }

        public static class PlayerPrefs
        {
            public static class User
            {
                public static readonly string Token = nameof(Token);
            }
        }

        public static class Popup
        {
            const string prefix = "Popup_";
            public static readonly string QuizzAnswerSelector = $"{prefix}{nameof(QuizzAnswerSelector)}";
            public static readonly string EventDetail = $"{prefix}{nameof(EventDetail)}";
            public static readonly string GameDetail = $"{prefix}{nameof(GameDetail)}";
            public static readonly string Login = $"{prefix}{nameof(Login)}";
            public static readonly string OTPInputPage = $"{prefix}{nameof(OTPInputPage)}";
            public static readonly string InputUserDetail = $"{prefix}{nameof(InputUserDetail)}";
            public static readonly string InputAccountDetail = $"{prefix}{nameof(InputAccountDetail)}";
            public static readonly string EventListView = $"{prefix}{nameof(EventListView)}";
            public static readonly string Search = $"{prefix}{nameof(Search)}";
            public static readonly string Voucher = $"{prefix}{nameof(Voucher)}";
            public static readonly string VoucherDetail = $"{prefix}{nameof(VoucherDetail)}";
            public static readonly string VoucherQR = $"{prefix}{nameof(VoucherQR)}";
        }

        public static class Scene
        {
            public static readonly string LandingScene = nameof(LandingScene);
            public static readonly string HomeScene = nameof(HomeScene);
            public static readonly string RealTimeQuizScene = nameof(RealTimeQuizScene);
        }
    }
}
