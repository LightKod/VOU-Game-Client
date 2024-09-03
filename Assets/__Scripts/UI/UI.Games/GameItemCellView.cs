using CodiceApp.EventTracking;
using EnhancedUI.EnhancedScroller;
using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class GameItemCellView : EnhancedScrollerCellView
    {
        [SerializeField] Image imgPoster;
        [SerializeField] TextMeshProUGUI txtGameName;

        GameModel gameModel;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenGameDetail);
        }


        async void OpenGameDetail()
        {
            await Task.Yield();
            if (gameModel == null) return;
            var gameDetailPopup = await PopupManager.instance.OpenUI<GameDetailPopup>(Keys.Popup.GameDetail, 0, false);
            gameDetailPopup.SetGameID(gameModel.id);
            gameDetailPopup.EnableUI();
        }

        public async void SetData(GameModel gameModel)
        {
            this.gameModel = gameModel;
            txtGameName.text = gameModel.name;
            imgPoster.sprite = await ImageCache.GetImage(gameModel.poster);
        }
    }
}
