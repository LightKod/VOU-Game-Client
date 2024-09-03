using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class GameScroller : ModelScroller<GameModel>
    {
        public void SetData(List<GameModel> gameModels)
        {
            _data = gameModels;
            scroller.ReloadData();
        }

        public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            GameModel model = _data[dataIndex];
            GameItemCellView cellView = scroller.GetCellView(cellViewPrefab) as GameItemCellView;
            cellView.SetData(model);
            return cellView;
        }
    }
}
