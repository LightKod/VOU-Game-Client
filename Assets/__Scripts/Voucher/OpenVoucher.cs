using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenVoucher : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenVoucher);
        }

        async void OnOpenVoucher()
        {
            VoucherPopup popup = await PopupManager.instance.OpenUI<VoucherPopup>(Keys.Popup.Voucher, 0);
        }
    }
}
