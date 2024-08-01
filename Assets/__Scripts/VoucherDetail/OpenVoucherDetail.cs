using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenVoucherDetail : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenVoucherDetail);
        }

        async void OnOpenVoucherDetail()
        {
            VoucherDetailPopup popup = await PopupManager.instance.OpenUI<VoucherDetailPopup>(Keys.Popup.VoucherDetail, 0);
        }
    }
}
