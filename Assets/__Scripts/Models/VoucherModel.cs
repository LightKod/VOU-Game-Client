using System;

namespace VOU
{
    public class VoucherModel : BaseModel
    {
        public string user_id { get; set; }
        public string qr { get; set; }
        public string status { get; set; }
        public int voucher_template_id { get; set; }
        public int brand_id { get; set; }
        public int event_id { get; set; }
        public int game_id { get; set; }
        public DateTime expiry_date { get; set; }

    }
}
