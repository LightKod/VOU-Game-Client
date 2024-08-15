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
            public static class Quiz
            {
                public static string Root = "/quiz";
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
                public static string GetWithID = $"{Root}";
            }
        }
    }
}
