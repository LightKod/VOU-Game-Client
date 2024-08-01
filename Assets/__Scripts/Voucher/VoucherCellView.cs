using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class VoucherCellView : EnhancedScrollerCellView
    {
        [SerializeField] private Image voucherImage;
        [SerializeField] private TMP_Text cellTitle;
        [SerializeField] private TMP_Text expiryDate;

        public void SetData(VoucherItem data)
        {
            cellTitle.text = data.voucherName;
            expiryDate.text = data.expiryDate;
        }
    }
}
