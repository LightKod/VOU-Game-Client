using Cysharp.Threading.Tasks;
using Owlet.UI.Popups;
using ScratchCardAsset;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class ScratchCardPopup : Popup
    {
        [SerializeField] TextMeshProUGUI txtBrandName;
        [SerializeField] TextMeshProUGUI txtItemName;
        [SerializeField] Image imgItemIcon;
        [SerializeField] ScratchCardManager scratchCard;

        [Header("Effects")]
        [SerializeField] ParticleSystem particleLeft;
        [SerializeField] ParticleSystem particleRight;
        GachaItemModel model;

        bool isFinished = true;

        private void Awake()
        {
            scratchCard.Progress.OnProgress += Progress_OnProgress;
        }

        public override async void EnableUI()
        {
            await UpdateUI(model);
            base.EnableUI();
        }

        private void Progress_OnProgress(float progress)
        {
            if(isFinished) return;
            if(progress > 0.7f)
            {
                FinishScratch();
            }
        }

        public void SetupUI(GachaItemModel model)
        {
            this.model = model;
        }

        async UniTask UpdateUI(GachaItemModel model)
        {
            txtItemName.text = model.name;
            imgItemIcon.sprite = await ImageCache.GetImage(model.img);
            isFinished = false;
        }


        protected override void OnDisableUI()
        {
            scratchCard.ResetScratchCard();
            base.OnDisableUI();
        }

        void FinishScratch()
        {
            isFinished = true;

            particleLeft.Play();
            particleRight.Play();

            MessageItemPopup.Open(model, () =>
            {
                SelfClosing();
            });
        }
    }
}
