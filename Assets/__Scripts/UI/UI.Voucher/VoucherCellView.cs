using Cysharp.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using Newtonsoft.Json;
using Owlet.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class VoucherCellView : EnhancedScrollerCellView
    {
        [SerializeField] private Image voucherImage;
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtExpiryDate;

        [SerializeField] bool interactable = false;

        VoucherModel voucherModel;
        VoucherTemplateModel voucherTemplateModel;

        private void Start()
        {
            if (interactable)
            {
                GetComponent<Button>().onClick.AddListener(OpenDetail);
            }
        }

        public async void SetData(VoucherModel data)
        {
            voucherModel = data;
            await FetchVoucherDetail(data);

        }

        public async void SetData(VoucherTemplateModel model)
        {
            voucherTemplateModel = model;

            voucherImage.sprite = await ImageCache.GetImage(model.image);
            txtName.text = model.name;
            txtExpiryDate.text = model.description;

            if(voucherModel != null)
            {
                txtExpiryDate.text = voucherModel.expire.ToString("dd-MM-yyyy");
            }
        }

        async void OpenDetail()
        {
            VoucherDetailPopup voucherDetailPopup = await PopupManager.instance.OpenUI<VoucherDetailPopup>(Keys.Popup.VoucherDetail, 0, false);
            await Task.Yield();
            if (voucherTemplateModel == null || voucherModel == null) return;
            voucherDetailPopup.SetData(voucherModel, voucherTemplateModel);
            voucherDetailPopup.EnableUI();
        }


        async UniTask FetchVoucherDetail(VoucherModel voucherModel)
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.Voucher.VoucherTemplate.GetByID}?id={voucherModel.voucher_template_id}"), true, async (res) =>
            {
                try
                {
                    VoucherTemplateModel voucherTemplateModel = JsonConvert.DeserializeObject<VoucherTemplateModel>(res);
                    SetData(voucherTemplateModel);
                    this.voucherTemplateModel = voucherTemplateModel;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error: {e}");
                }

            },
            (msg) =>
            {
                Debug.Log($"Fetch game data failed: {msg}");
            });
        }
    }
}
