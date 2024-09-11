using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class GachaInventoryModel : BaseModel
    {
        public int game_id { get; set; }
        public string user_id { get; set; }
        public string item { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
