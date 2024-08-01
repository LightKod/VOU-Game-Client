using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenVoucherQR : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenVoucherQR);
        }

        async void OnOpenVoucherQR()
        {
            VoucherQRPopup popup = await PopupManager.instance.OpenUI<VoucherQRPopup>(Keys.Popup.VoucherQR, 0);
        }
    }
}
