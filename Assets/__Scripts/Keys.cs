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
        }

        public static class Popup
        {
            const string prefix = "Popup_";
            public static readonly string QuizzAnswerSelector = $"{prefix}{nameof(QuizzAnswerSelector)}";
            public static readonly string EventDetail = $"{prefix}{nameof(EventDetail)}";
        }
    }
}
