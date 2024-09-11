using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class GachaItemCellView : EnhancedScrollerCellView
    {
        [SerializeField] Image imgIcon;
        [SerializeField] TextMeshProUGUI txtItemName;
        [SerializeField] TextMeshProUGUI txtItemDescription;
        [SerializeField] TextMeshProUGUI txtItemCount;

        public async void SetupUI(InventoryModel model)
        {
            txtItemName.text = model.item.name;
            txtItemDescription.text = model.item.description;
            txtItemCount.text = $"x{model.amount}";

            imgIcon.sprite = await ImageCache.GetImage(model.item.img);
        }
    }
}
