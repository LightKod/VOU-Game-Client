using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Owlet;
using Owlet.Systems.SceneTransistions;
using Owlet.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static VOU.Env.Routes;

namespace VOU
{
    public class GachaManager : Singleton<GachaManager>
    {
        [SerializeField] string tempToken;
        [SerializeField] int tempGameID;

        [SerializeField] Button btnScratch;
        [SerializeField] Button btnInventory;
        [SerializeField] Button btnExchange;
        [SerializeField] TextMeshProUGUI txtScratchAmount;

        int gameID;

        int playCount;


        GameModel gameModel;

        private async void Start()
        {
            btnScratch.onClick.AddListener(Scratch);
            btnInventory.onClick.AddListener(OpenInventory);
            btnExchange.onClick.AddListener(Exchange);

            await FetchGameData();
            await FetchPlayTime();
            FinishFetch();
        }

        protected override void Init()
        {
            base.Init();
            //gameID = PlayerPrefs.GetInt(Keys.PlayerPrefs.GameID);
            gameID = tempGameID;
            PlayerPrefs.SetString(Keys.PlayerPrefs.User.Token, tempToken);
        }


        void Scratch()
        {
            UsePlayTime();
        }

        async void OpenInventory()
        {
            InventoryPopup inventoryPopup = await PopupManager.instance.OpenUI<InventoryPopup>(Keys.Popup.Inventory, 1, false);
            inventoryPopup.SetData(gameID);
            inventoryPopup.EnableUI();
        }

        async void Exchange()
        {
            ItemExchangePopup itemExchangePopup = await PopupManager.instance.OpenUI<ItemExchangePopup>(Keys.Popup.ExchangeItem, 1, false);
            itemExchangePopup.SetData(gameModel);
            itemExchangePopup.EnableUI();
        }

        void FinishFetch()
        {
            SceneTransistion.instance.DisableLoadingScreen();
        }

        void UpdateScratchAmount(int newAmount)
        {
            playCount = newAmount;
            txtScratchAmount.text = $"SCRATCH\n({playCount} REMAINS)";
        }

        async UniTask FetchGameData()
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.Game.GetWithID}/{gameID}"), true, (res) =>
            {
                try
                {
                    gameModel = JsonConvert.DeserializeObject<GameModel>(res);
                }
                catch (Exception e)
                {
                    gameModel = null;
                    Debug.LogError($"Error: {e}");
                }
            },
            (msg) =>
            {
                Debug.Log($"Fetch game data failed: {msg}");
            });
        }

        async UniTask FetchPlayTime()
        {
            await HttpClient.GetRequest(ServiceHelper.GetURL(Env.Routes.GachaPlayer.PlayTime) + $"/{gameID}", true, (string res) =>
            {
                var resObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);

                string msgStr = resObject["message"].ToString();
                string playCountStr = resObject["play_duration"].ToString();

                int playCount = int.Parse(playCountStr);

                UpdateScratchAmount(playCount);

                Debug.Log($">>> Playcount: {playCount}");

            },
            (string error) =>
            {
                Debug.Log(error);
            });
        }

        async void UsePlayTime()
        {
            btnScratch.enabled = false;

            Dictionary<string, string> form = new()
            {
                { "gameId", gameModel.id.ToString()},
                { "gameDataId", gameModel.game_data_id.ToString()},
            };

            await HttpClient.PostRequest(HttpClient.GetURL(Env.Routes.GachaPlayer.UsePlay), form, true, async (string res) =>
            {
                var resObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                string msgStr = resObj["message"].ToString();
                string itemStr = resObj["item"].ToString();
                string remainPlayTimeStr = resObj["remainingPlayTime"].ToString();

                int playCount = int.Parse(remainPlayTimeStr);
                UpdateScratchAmount(playCount);

                GachaItemModel item = JsonConvert.DeserializeObject<GachaItemModel>(itemStr);

                Debug.Log($"Get item: {item.name}");

                ScratchCardPopup scratchCardPopup = await PopupManager.instance.OpenUI<ScratchCardPopup>(Keys.Popup.ScratchCard, 1, false);
                scratchCardPopup.SetupUI(item);
                scratchCardPopup.onClosed += () =>
                {
                    btnScratch.enabled = true;
                };
                scratchCardPopup.EnableUI();

            },
            (string error) =>
            {
                btnScratch.enabled = true;
                Debug.Log($"Error play: {error}");
            });

        }



    }
}
