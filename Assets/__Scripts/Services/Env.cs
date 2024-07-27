using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    //Should realy not exist here
    public static class Env
    {
        public static string BASE_URL = "http://localhost:3000";

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
        }




    }
}
