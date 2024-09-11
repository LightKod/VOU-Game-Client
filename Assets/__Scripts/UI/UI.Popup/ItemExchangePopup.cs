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

namespace VOU
{
    public class ItemExchangePopup : Popup
    {
        GameModel gameModel;
        Dictionary<string, int> invenItemDictionary;

        List<GachaItemModel> itemDatas;
        List<ItemSetModel> itemSets;


        public void SetData(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }


        protected override async UniTask FetchData()
        {
            await FetchInventory();
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

            await HttpClient.PostRequest(HttpClient.GetURL($"{Env.Routes.Gacha.GetItems}"), form, true, (res) =>
            {
                itemDatas = JsonConvert.DeserializeObject<List<GachaItemModel>>(res);
                Debug.Log(itemDatas.Count);
            },
            (msg) =>
            {
                Debug.Log($"Fetch game data failed: {msg}");
            });
        }

    }
}
