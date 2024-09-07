using Lean.Gui;
using Owlet.UI;
using Owlet.UI.Popups;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace VOU
{
    public class BottomNavigationButton : MonoBehaviour
    {
        [SerializeField] string popupID;
        LeanToggle toggle;

        static string currentPopupId = "";

        static BottomNavigationButton currentSelection;

        private void Start()
        {
            toggle = GetComponent<LeanToggle>();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        async void OnClick()
        {
            if (currentSelection == this) return;
            PopupManager.instance.CloseUI(currentPopupId);
            toggle.TurnOn();
            if (!popupID.IsNullOrWhitespace())
            {
                await PopupManager.instance.OpenUI<Popup>(popupID, 0);
            }
            currentPopupId = popupID;
            currentSelection = this;
        }
    }
}
