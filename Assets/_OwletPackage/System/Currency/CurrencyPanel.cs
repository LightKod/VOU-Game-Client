using DG.Tweening;
using Owlet.Systems.Currency;
using Owlet.Systems.SaveLoad;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Owlet.UI
{
    public class CurrencyPanel : MonoBehaviour
    {
        [SerializeField] protected CurrencyType currencyType;
        [SerializeField] TextMeshProUGUI txtAmount;
        [SerializeField] Image icon;

        const float ANIMATION_TIME = 1f;

        Sequence bounceSequence;

        private void Awake()
        {
            CurrencyManager.onResourceGained += UpdateUI;
            CurrencyManager.onResourceUsed += UpdateUI;
            SaveManager.onDataLoaded += SetInitialText;

            icon.sprite = currencyType.icon;

            bounceSequence = DOTween.Sequence()
                            .Append(icon.transform.DOScale(Vector3.one * 1.2f, 0.1f))
                            .Append(icon.transform.DOScale(Vector3.one * 0.8f, 0.2f))
                            .Append(icon.transform.DOScale(Vector3.one * 1f, 0.1f))
                            .SetAutoKill(false)
                            .Pause();

        }

        private void Start()
        {
            txtAmount.text = CurrencyManager.instance.GetResource(currencyType.currencyName).ToString();
        }

        void SetInitialText()
        {
            txtAmount.text = CurrencyManager.instance.GetResource(currencyType.currencyName).ToString();
        }


        private void OnDestroy()
        {
            CurrencyManager.onResourceGained -= UpdateUI;
            CurrencyManager.onResourceUsed -= UpdateUI;
            SaveManager.onDataLoaded -= SetInitialText;
        }

        private void UpdateUI(CurrencyType type, int amount, string source)
        {
            if (type != currencyType) return;
            /*DOTween.Kill(gameObject);
            int currentAmount = int.Parse(txtAmount.text);
            DOVirtual.Float(currentAmount, amount, ANIMATION_TIME, (float value) =>
            {
                txtAmount.text = value.ToString();
            }).SetTarget(gameObject);*/
            txtAmount.text = amount.ToString();
            bounceSequence.Restart();
        }
    }
}
