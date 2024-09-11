using Owlet;
using Owlet.UI;
using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class VoucherDetailPopup : Popup
    {
        [SerializeField] TextMeshProUGUI txtVoucherName;
        [SerializeField] TextMeshProUGUI txtVoucherDescription;
        [SerializeField] TextMeshProUGUI txtVoucherExpiryDate;
        [SerializeField] Button btnUseVoucher;

        [SerializeField] RefreshRectTransform refresher;

        VoucherModel voucherModel;
        VoucherTemplateModel voucherTemplateModel;

        private void Awake()
        {
            btnUseVoucher.onClick.AddListener(UseVoucher);
        }

        public void SetData(VoucherModel voucherModel, VoucherTemplateModel voucherTemplateModel)
        {
            this.voucherModel = voucherModel;
            this.voucherTemplateModel = voucherTemplateModel;

            SetupUI();
        }

        async void SetupUI()
        {
            txtVoucherName.text = voucherTemplateModel.name;
            txtVoucherDescription.text = voucherTemplateModel.description;
            txtVoucherExpiryDate.text = $"Expiry data: {voucherModel.expire.ToString("dd-MM-yyyy")}";
            await Task.Delay(500);
            refresher.Refresh();
        }

        async void UseVoucher()
        {
            VoucherQRPopup voucherQRPopup = await PopupManager.instance.OpenUI<VoucherQRPopup>(Keys.Popup.VoucherQR, 0, false);
            
            voucherQRPopup.EnableUI();
        }
    }
}
