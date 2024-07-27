using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class ServiceHelper : MonoBehaviour
    {
        public static string GetURL(string baseUrl, string route)
        {
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            if (route.StartsWith("/"))
            {
                route = route.Substring(1);
            }

            return baseUrl + route;
        }

        public static string GetURL(string route)
        {
            return GetURL(Env.BASE_URL, route);
        }
    }
}
