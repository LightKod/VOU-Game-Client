using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Owlet.UI.Popups;
using Cysharp.Threading.Tasks;
using System.Linq;
using System;
using Newtonsoft.Json;
using static VOU.Env.Routes;
using UnityEngine.UIElements;
using EnhancedUI.EnhancedScroller;
using System.Threading.Tasks;

namespace VOU
{
    public class ItemExchangePopup : Popup, IEnhancedScrollerDelegate
    {
        [SerializeField] EnhancedScroller scroller;
        [SerializeField] EnhancedScrollerCellView cellViewPrefab;

        GameModel gameModel;
        Dictionary<string, int> invenItemDictionary = new();

        List<GachaItemModel> itemDatas = new();
        List<ItemSetModel> itemSets = new();
        VoucherTemplateModel voucherTemplateModel;

        private void Awake()
        {
            scroller.Delegate = this;
        }
        public void SetData(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }


        protected override async UniTask FetchData()
        {
            await FetchVoucherDetail();
        }

        async UniTask FetchVoucherDetail()
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.Voucher.VoucherTemplate.GetByID}?id={gameModel.voucher_template_id}"), true, async (res) =>
            {
                try
                {
                    voucherTemplateModel = JsonConvert.DeserializeObject<VoucherTemplateModel>(res);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error: {e}");
                }
                await FetchInventory();
            },
            (msg) =>
            {
                Debug.Log($"Fetch game data failed: {msg}");
            });
        }

        async UniTask FetchInventory()
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.GachaPlayer.Inventory}/{gameModel.id}"), true, async (res) =>
            {
                try
                {
                    List<GachaInventoryModel> invenList = JsonConvert.DeserializeObject<List<GachaInventoryModel>>(res);
                    invenItemDictionary = invenList.GroupBy(x => x.item)
                                                   .ToDictionary(g => g.Key, g => g.Count());

                    await FetchItemSet();
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


        async UniTask FetchItemSet()
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.Gacha.GetSetsByGameID}/{gameModel.game_data_id}"), true, async (res) =>
            {
                try
                {
                    var resObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    string itemSetArrayStr = resObj["itemSets"].ToString();
                    Debug.Log(itemSetArrayStr);
                    itemSets = JsonConvert.DeserializeObject<List<ItemSetModel>>(itemSetArrayStr);

                    foreach( ItemSetModel itemSet in itemSets )
                    {
                        Debug.Log($"{itemSet.name}: {itemSet.items.Count}");
                    }

                    await FetchItemData();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error: {e}");
                }
            },
            (msg) =>
            {
                Debug.Log($"Fetch item set failed: {msg}");
            });
        }


        async UniTask FetchItemData()
        {
            List<string> uniqueItemID = itemSets.SelectMany(model => model.items).Distinct().ToList();
            string itemIdsJson = JsonConvert.SerializeObject(uniqueItemID);
            Dictionary<string, string> form = new()
            {
                { "itemIds", itemIdsJson}
            };

            await HttpClient.PostRequest(HttpClient.GetURL($"{Env.Routes.Gacha.GetItems}"), form, true, async (res) =>
            {
                itemDatas = JsonConvert.DeserializeObject<List<GachaItemModel>>(res);
                Debug.Log(itemDatas.Count);

                InitScroller();
            },
            (msg) =>
            {
                Debug.Log($"Fetch game data failed: {msg}");
            });
        }


        

        async void InitScroller()
        {
            await Task.Yield();
            scroller.ReloadData();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return itemSets.Count();
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 300;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            ItemExchangeCellView cellView = scroller.GetCellView(cellViewPrefab) as ItemExchangeCellView;
            cellView.SetupUI(gameModel, itemSets[dataIndex], invenItemDictionary, voucherTemplateModel,itemDatas);
            return cellView;
        }
    }
}
