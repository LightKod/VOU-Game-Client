using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Owlet;
using Owlet.Systems.SceneTransistions;
using Owlet.UI.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class GameDetailPopup : Popup
    {
        [Header("Game")]
        [SerializeField] Image imgPoster;
        [SerializeField] TextMeshProUGUI txtGameName;
        [SerializeField] TextMeshProUGUI txtGameDetail;
        [SerializeField] TextMeshProUGUI txtStartDate;
        [SerializeField] TextMeshProUGUI txtEndDate;

        [Header("Game Type")]
        [SerializeField] TextMeshProUGUI txtGameTypeName;
        [SerializeField] Button btnPlay;
        [Header("Others")]
        [SerializeField] RefreshRectTransform refresher;

        int gameId;

        GameModel gameModel;
        GameTypeModel gameTypeModel;

        private void Awake()
        {
            btnPlay.onClick.AddListener(PlayGame);
        }

        public void SetGameID(int gameId)
        {
            this.gameId = gameId;
        }

        protected override async UniTask FetchData()
        {
            await FetchGameData(gameId);
        }


        async UniTask FetchGameData(int gameId)
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.Game.GetWithID}/{gameId}"),true, async (res) =>
            {
                try
                {
                    gameModel = JsonConvert.DeserializeObject<GameModel>(res);
                    await FetchGameTypeData(gameModel.game_type_id);
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

        async UniTask FetchGameTypeData(int gameTypeId)
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.Game.GetWithID}/{gameTypeId}"),true, (res) =>
            {
                try
                {
                    gameTypeModel = JsonConvert.DeserializeObject<GameTypeModel>(res);
                }
                catch (Exception e)
                {
                    gameTypeModel = null;
                    Debug.LogError($"Error: {e}");
                }
                UpdateUI(gameModel, gameTypeModel);
            },
            (msg) =>
            {
                Debug.Log($"Fetch game type data failed: {msg}");
                UpdateUI(gameModel, gameTypeModel);
            });
        }

        async void UpdateUI(GameModel gameModel, GameTypeModel gameTypeModel)
        {
            imgPoster.sprite = await ImageCache.GetImage(gameModel.poster);

            txtGameName.text = gameModel.name;
            txtGameDetail.text = gameModel.description;

            txtStartDate.text = gameModel.start_time.ToString("dd/MM/yyyy");
            txtEndDate.text = gameModel.end_time.ToString("dd/MM/yyyy");

            refresher.Refresh();

            if (gameTypeModel == null) return;
            txtGameTypeName.text = "Real-time Quiz";

            refresher.Refresh();
        }


        void PlayGame()
        {
            if (gameModel == null) return;
            DateTime now = DateTime.Now;
            if(now > gameModel.end_time)
            {
                MessagePopup.Open("Game has ended", "The game has already ended!");

                return;
            }
            HandleGameSceneTransition();
        }


        void HandleGameSceneTransition()
        {
            PlayerPrefs.SetInt(Keys.PlayerPrefs.GameID, gameId);

            if(gameTypeModel.id == 1) //Real time quiz
            {
                SceneTransistion.instance.ChangeScene(Keys.Scene.RealTimeQuizScene);
            }

        }
    }
}
