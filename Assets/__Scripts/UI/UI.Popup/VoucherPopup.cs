using Cysharp.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using Newtonsoft.Json;
using Owlet.UI.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class VoucherPopup : Popup, IEnhancedScrollerDelegate
    {
        [SerializeField] EnhancedScrollerCellView cellViewPrefab;
        [SerializeField] EnhancedScroller scroller;

        List<VoucherModel> vouchers = new();

        private void Awake()
        {
            scroller.Delegate = this;
        }

        protected async override void OnEnableUI()
        {
            await FetchVouchers();
            base.OnEnableUI();
        }


        async UniTask FetchVouchers()
        {
            await HttpClient.GetRequest(ServiceHelper.GetURL(Env.Routes.Voucher.GetByUserId), true, (string res) =>
            {
                try
                {
                    var vouchers = JsonConvert.DeserializeObject<List<VoucherModel>>(res);

                    SetData(vouchers);
                }
                catch(Exception e)
                {
                    Debug.LogException(e);
                }
          
            },
            (string msg) =>
            {
                Debug.Log($"Error fetch voucher: {msg}");
            });
        }

        void SetData(List<VoucherModel> vouchers)
        {
            this.vouchers = vouchers;
            scroller.ReloadData();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return vouchers.Count;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 180;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            VoucherModel model = vouchers[dataIndex];
            VoucherCellView cellView = scroller.GetCellView(cellViewPrefab) as VoucherCellView;
            cellView.SetData(model);
            return cellView;
        }
    }
}
