using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class VoucherTemplateModel : BaseModel
    {
        public string name { get; set; }
        public string image { get; set; }
        public float value { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
