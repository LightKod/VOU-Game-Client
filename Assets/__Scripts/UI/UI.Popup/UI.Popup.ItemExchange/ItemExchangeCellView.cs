using EnhancedUI.EnhancedScroller;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class ItemExchangeCellView : EnhancedScrollerCellView, IEnhancedScrollerDelegate
    {
        [SerializeField] TextMeshProUGUI txtVoucherName;
        [SerializeField] Button btnExchange;
        [SerializeField] EnhancedScroller scroller;
        [SerializeField] EnhancedScrollerCellView cellViewPrefab;
        ItemSetModel itemSetModel;
        GameModel gameModel;

        List<InventoryModel> inventoryModels = new();
        private void Awake()
        {
            scroller.Delegate = this;
            btnExchange.onClick.AddListener(Exchange);
        }
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            GachaItemCellView cellView = scroller.GetCellView(cellViewPrefab) as GachaItemCellView;
            cellView.SetupUI(inventoryModels[dataIndex]);
            return cellView;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 164;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return inventoryModels.Count;
        }

        public void SetupUI(GameModel gameModel, ItemSetModel itemSetModel, Dictionary<string, int> invenItemDictionary, VoucherTemplateModel voucherTemplateModel, List<GachaItemModel> itemDatas)
        {
            this.itemSetModel = itemSetModel;
            this.gameModel = gameModel;
            if (voucherTemplateModel != null)
            {
                txtVoucherName.text = voucherTemplateModel.name;
            }

            if(itemSetModel != null)
            {
                inventoryModels = new();
                foreach(string itemId in itemSetModel.items)
                {
                    int amount = invenItemDictionary.ContainsKey(itemId) ? invenItemDictionary[itemId] : 0;
                    GachaItemModel gachaItemModel = itemDatas.FirstOrDefault(x => x._id == itemId);

                    inventoryModels.Add(new()
                    {
                        item = gachaItemModel,
                        amount = amount
                    });
                }

                scroller.ReloadData();
            }
        }

        async void Exchange()
        {
            Dictionary<string, string> form = new()
            {
                { "gameId", gameModel.id.ToString()},
                { "itemSetId", itemSetModel._id.ToString()}
            };

            await HttpClient.PostRequest(HttpClient.GetURL($"{Env.Routes.GachaPlayer.Redeem}"), form, true, async (res) =>
            {
                MessagePopup.Open("Redeem successfully", "Check your voucher on the main menu");
            },
            (msg) =>
            {
                MessagePopup.Open("Redeem failed", "You don't have the required item");
            });
        }
    }
}
