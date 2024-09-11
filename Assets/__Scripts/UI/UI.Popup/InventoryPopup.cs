using Cysharp.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using Google.MiniJSON;
using Newtonsoft.Json;
using Owlet.UI.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static VOU.Env.Routes;

namespace VOU
{
    public class InventoryPopup : Popup, IEnhancedScrollerDelegate
    {
        [SerializeField] EnhancedScroller scroller;
        [SerializeField] EnhancedScrollerCellView cellViewPrefab;
        
        List<InventoryModel> _data = new();
        Dictionary<string, int> invenItemDictionary;

        int gameID;

        private void Awake()
        {
            scroller.Delegate = this;
        }


        protected override async UniTask FetchData()
        {
            await FetchInventory();
        }

        public void SetData(int gameID)
        {
            this.gameID = gameID;
        }


        async UniTask FetchInventory()
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.GachaPlayer.Inventory}/{gameID}"), true, async (res) =>
            {
                try
                {
                    List<GachaInventoryModel> invenList = JsonConvert.DeserializeObject<List<GachaInventoryModel>>(res);
                    invenItemDictionary = invenList.GroupBy(x => x.item)
                                                   .ToDictionary(g => g.Key, g => g.Count());
                    Debug.Log(invenList.Count);

                    await FetchItemDetail();
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


        async UniTask FetchItemDetail()
        {
            List<string> uniqueItemID = invenItemDictionary.Keys.ToList();
            string itemIdsJson = JsonConvert.SerializeObject(uniqueItemID);

            Dictionary<string, string> form = new()
            {
                { "itemIds", itemIdsJson}
            };

            await HttpClient.PostRequest(HttpClient.GetURL($"{Env.Routes.Gacha.GetItems}"), form, true, (res) =>
            {
                List<GachaItemModel> gachaItems = JsonConvert.DeserializeObject<List<GachaItemModel>>(res);

                _data = new();

                foreach (GachaItemModel item in gachaItems)
                {
                    _data.Add(new InventoryModel()
                    {
                        item = item,
                        amount = invenItemDictionary[item._id]
                    });
                }

                scroller.ReloadData();

            },
            (msg) =>
            {
                Debug.Log($"Fetch game data failed: {msg}");
            });
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            Debug.Log($"Item: {dataIndex * 2} | {dataIndex * 2 + 1}");


            InventoryModel model_1 = _data[dataIndex * 2];
            InventoryModel model_2 = null;

            Debug.Log(model_1.item.name);

            if(dataIndex * 2 + 1 < _data.Count)
            {
                model_2 = _data[dataIndex * 2 + 1];
            }

            InventoryCellView cellView = scroller.GetCellView(cellViewPrefab) as InventoryCellView;
            cellView.SetupUI(model_1, model_2);
            return cellView;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 660;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            int count = Mathf.CeilToInt(_data.Count / 2f);
            Debug.Log($"Row count: {count}");
            return count;
        }
    }
}
