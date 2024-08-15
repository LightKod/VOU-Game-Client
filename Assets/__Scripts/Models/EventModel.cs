using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class EventModel : BaseModel
    {
        public int brand_id { get; set; } 
        public string poster { get; set; }
        public string name { get; set; }  
        public string description { get; set; }  
        public DateTime start_time { get; set; }  
        public DateTime end_time { get; set; } 
        public DateTime created_at { get; set; }  
        public DateTime updated_at { get; set; }  
    }
}
