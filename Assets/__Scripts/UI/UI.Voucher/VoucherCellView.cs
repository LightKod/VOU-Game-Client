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
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtExpiryDate;

        [SerializeField] bool interactable = false;

        private void Start()
        {
            if (interactable)
            {
                GetComponent<Button>().onClick.AddListener(OpenDetail);
            }
        }

        public void SetData(VoucherModel data)
        {
            txtName.text = "Test";
            txtExpiryDate.text = "15/09/2024";
        }

        public async void SetData(VoucherTemplateModel model)
        {
            voucherImage.sprite = await ImageCache.GetImage(model.image);
            txtName.text = model.name;
            txtExpiryDate.text = model.description;
        }

        void OpenDetail()
        {

        }
    }
}
