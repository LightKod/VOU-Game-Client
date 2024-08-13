using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public enum ToastState
    {
        Error, Success
    }

    public class ToastItem : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txtMsg;
        [SerializeField] Image bg;
        [SerializeField] CanvasGroup canvasGroup;

        public void SetupUI(string txt,ToastState state ,Vector3 startPos, Vector3 endPos)
        {
            txtMsg.text = txt;
            transform.localPosition = startPos;
            canvasGroup.alpha = 1f;

            bg.color = state switch
            {
                ToastState.Error => new Color32(247, 84, 85, 255),
                ToastState.Success => new Color32(18,209,142,255),
                _ =>  new Color32(0,0,0,255),
            };

            var sequence = DOTween.Sequence()
                .Append(transform.DOLocalMove(endPos, 0.5f))
                .AppendInterval(1f)
                .Append(canvasGroup.DOFade(0, 0.3f))
                .AppendCallback(() =>
                {
                    LeanPool.Despawn(gameObject);
                })
                .SetTarget(this);
        }


        public void CloseInstantly()
        {
            this.DOKill();
            LeanPool.Despawn(gameObject);
        }
    }
}
