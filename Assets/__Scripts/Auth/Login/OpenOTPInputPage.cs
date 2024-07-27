using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenOTPInputPage : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenOTPInputPage);
        }

        async void OnOpenOTPInputPage()
        {
            OTPInputPagePopup popup = await PopupManager.instance.OpenUI<OTPInputPagePopup>(Keys.Popup.OTPInputPage, 0);
        }
    }
}
