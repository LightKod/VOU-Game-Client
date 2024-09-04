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
    public class MessagePopup : Popup
    {
        [SerializeField] TextMeshProUGUI txtTitle;
        [SerializeField] TextMeshProUGUI txtDetail;
        [SerializeField] Button btnCancel;
        [SerializeField] Button btnOk;

        Action onCancel;
        Action onOK;

        private void Start()
        {
            btnCancel.onClick.AddListener(OnCacelClick);
            btnOk.onClick.AddListener(OnOKClick);
        }

        public static async void Open(string title, string detail, Action onOK = null, Action onCancel = null)
        {
            MessagePopup messagePopup = await PopupManager.instance.OpenUI<MessagePopup>(Keys.Popup.MessageBox, 2, false);
            messagePopup.SetupUI(title, detail, onOK, onCancel);
            messagePopup.EnableUI();
        }

        public void SetupUI(string title, string detail, Action onOK = null, Action onCancel = null) 
        {
            this.onCancel = onCancel;
            this.onOK = onOK;

            txtTitle.text = title;
            txtDetail.text = detail;

            btnCancel.gameObject.SetActive(onCancel != null);
        }


        void OnOKClick()
        {
            onOK?.Invoke();
            SelfClosing();
        }


        void OnCacelClick()
        {
            onCancel?.Invoke();
            SelfClosing();
        }
    }
}
