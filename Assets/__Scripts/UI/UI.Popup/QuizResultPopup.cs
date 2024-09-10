using EnhancedUI.EnhancedScroller;
using Owlet.Systems.SceneTransistions;
using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class QuizResultPopup : Popup, IEnhancedScrollerDelegate
    {
        List<VictoryPlayer> _data;
        VoucherTemplateModel voucherTemplateModel;

        [SerializeField] EnhancedScroller scroller;
        [SerializeField] EnhancedScrollerCellView cellViewPrefab;
        [SerializeField] VoucherCellView voucherCellView;

        [SerializeField] Button btnReturn;


        private void Start()
        {
            scroller.Delegate = this;
            btnReturn.onClick.AddListener(ReturnToMainMenu);
        }

        public void SetData(List<VictoryPlayer> victoryPlayers, VoucherTemplateModel voucherTemplateModel)
        {
            _data = victoryPlayers;
            this.voucherTemplateModel = voucherTemplateModel;
            voucherCellView.SetData(voucherTemplateModel);
            scroller.ReloadData();
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            VictoryPlayer model = _data[dataIndex];
            VictoryPlayerCellView cellView = scroller.GetCellView(cellViewPrefab) as VictoryPlayerCellView;
            cellView.SetData(model);
            return cellView;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 100;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count;
        }

        void ReturnToMainMenu()
        {
            SceneTransistion.instance.ChangeScene(Keys.Scene.HomeScene);
        }
    }
}
