using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class ItemSetModel
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<string> items { get; set; }
    }
}
