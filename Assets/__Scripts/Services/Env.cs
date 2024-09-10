using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    //Should realy not exist here
    public static class Env
    {
        public static string BASE_URL = "http://192.168.191.173:8000";

        public static class Routes
        {
            public static class Socket
            {
                public static string Root = "/socket";
                public static string Quiz = $"";
            }
            public static class Chat
            {
                public static string Root = "/chat";
            }

            public static class Auth
            {
                public static string Root = "/auth";
                public static string Register = $"{Root}/register";
                public static string Login = $"{Root}/login";
            }

            public static class Event
            {
                public static string Root = "/event";
                public static string All = $"{Root}/all";
                public static string Search = $"{Root}/search";
                public static string GetWithID = $"{Root}";
            }

            public static class Game
            {
                public static string Root = "/game";
                public static string All = $"{Root}/all";
                public static string GetWithID = $"{Root}";
                public static string GetAllInEvent = $"{Root}/event";

                public static class GameType
                {
                    public static string Root = $"{Game.Root}/gametype";
                    public static string GetWithID = $"{Root}";

                }
            }

            public static class Voucher
            {

                public static class VoucherTemplate
                {

                }
            }
        }
    }
}
