using Owlet.UI;
using Owlet.UI.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class MessageItemPopup : Popup
    {
        [SerializeField] Image imgItem;
        [SerializeField] TextMeshProUGUI txtTitle;
        [SerializeField] TextMeshProUGUI txtDetail;
        [SerializeField] Button btnInventory;
        [SerializeField] Button btnOk;

        Action onClose;

        private void Start()
        {
            btnInventory.onClick.AddListener(OpenInventory);
            btnOk.onClick.AddListener(SelfClosing);
        }

        public static async void Open(GachaItemModel gachaItemModel, Action onClose = null)
        {
            MessageItemPopup messagePopup = await PopupManager.instance.OpenUI<MessageItemPopup>(Keys.Popup.MessageItem, 2, false);
            messagePopup.SetupUI(gachaItemModel, onClose);
            messagePopup.EnableUI();
        }

        public async void SetupUI(GachaItemModel gachaItemModel, Action onClose = null)
        {
            this.onClose = onClose;

            txtTitle.text = "Congratulation!";
            txtDetail.text = $"You recieved 1 {gachaItemModel.name}";

            imgItem.sprite = await ImageCache.GetImage(gachaItemModel.img);
        }

        void OpenInventory()
        {
            Debug.Log("Open inven");
            SelfClosing();
        }

        protected override void OnDisableUI()
        {
            onClose?.Invoke();
            onClose = null;
            base.OnDisableUI();
        }
    }
}
