using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    //Should realy not exist here
    public static class Env
    {
        public static string BASE_URL = "http://localhost:8000";

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
                public static string Register = $"{Root}/register/user";
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

            public static class GachaPlayer
            {
                public static string Root = "/gacha-player";
                public static string PlayTime = $"{Root}/playtime";
                public static string UsePlay = $"{Root}/use-play";
                public static string Inventory = $"{Root}/inventory";
                public static string Redeem = $"{Root}/redeem";
            }

            public static class Gacha
            {
                public static string Root = "/gacha";
                public static string GetItems = $"{Root}/items";
                public static string GetSetsByGameID = $"{Root}/itemSetByGameID";
            }

            public static class Voucher
            {
                public static string GetByUserId = "voucher/voucher/getByUserID";
                public static class VoucherTemplate
                {
                    public static string GetByID = "voucher/voucherTemplate/getByID";
                }
            }
        }
    }
}
