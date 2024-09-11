using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class InventoryCellView : EnhancedScrollerCellView
    {
        [SerializeField] GachaItemCellView cell_1;
        [SerializeField] GachaItemCellView cell_2;


        public void SetupUI(InventoryModel model_1, InventoryModel model_2)
        {
            cell_1.SetupUI(model_1);
            if(model_2 == null)
            {
                cell_2.gameObject.SetActive(false);
                return;
            }
            cell_2.SetupUI(model_2);
        }
    }
}
