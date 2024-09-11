using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class GameModel : BaseModel
    {
        public string brand_id { get; set; }
        public int? event_id { get; set; }
        public string poster { get; set; }
        public string name { get; set; }
        public int game_type_id { get; set; }
        public string game_data_id { get; set; }
        public bool tradable { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        public int voucher_template_id { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
