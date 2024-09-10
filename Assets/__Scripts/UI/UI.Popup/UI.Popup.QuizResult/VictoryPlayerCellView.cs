using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class VictoryPlayerCellView : EnhancedScrollerCellView
    {
        [SerializeField] Image imgAvatar;
        [SerializeField] TextMeshProUGUI txtName;

        
        public async void SetData(VictoryPlayer victoryPlayer)
        {
            txtName.text = victoryPlayer.email;
        }
    }
}
