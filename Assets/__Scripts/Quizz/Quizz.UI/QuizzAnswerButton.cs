using DG.Tweening;
using Lean.Gui;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public enum QuizzAnswerButtonState
    {
        Default = 0,
        Selected = 1,
        Correct = 2,
        Wrong = 3,
    }

    public class QuizzAnswerButton : MonoBehaviour
    {
        [ChildGameObjectsOnly][SerializeField] TextMeshProUGUI txtAnswer;
        [ChildGameObjectsOnly][SerializeField] TextMeshProUGUI txtAnswerCount;
        [ChildGameObjectsOnly][SerializeField] Slider sliderCorrect;
        [ChildGameObjectsOnly][SerializeField] Button btnSelect;
        [SerializeField] LeanSwitch states;
        int answerIndex;
        private void Awake()
        {
            QuizManager.instance.onAnswerSelected += DisableButtons;
            QuizzTimer.onCounterFinish += DisableButtons;
            btnSelect.onClick.AddListener(Select);
        }

        private void OnDestroy()
        {
            QuizManager.instance.onAnswerSelected -= DisableButtons;
            QuizzTimer.onCounterFinish += DisableButtons;
        }

        [Button]
        public void SetDefaultState(int answerIndex, string answer)
        {
            this.DOKill();

            this.answerIndex = answerIndex;
            txtAnswer.text = answer;
            sliderCorrect.value = 0;
            txtAnswerCount.text = "";
            SetState(QuizzAnswerButtonState.Default);
            btnSelect.enabled = true;
            btnSelect.targetGraphic.color = btnSelect.colors.normalColor;
        }

        public void SetResultState(int answerIndex, string answer, AnswerObject answerObject)
        {
            this.DOKill();
            txtAnswer.text = answer;
            this.answerIndex = answerIndex;

            if (answerIndex == answerObject.resultIndex)
            {
                SetState(QuizzAnswerButtonState.Correct);
            }
            else
            {
                SetState(QuizzAnswerButtonState.Wrong);
            }
            int sameAnswerCount = answerObject.answerCounts[answerIndex];

            Sequence animation = DOTween.Sequence()
                .AppendInterval(0.5f)
                .Append(DOVirtual.Int(0, sameAnswerCount, 1f, (int value) =>
                {
                    sliderCorrect.value = value * 1f / answerObject.answerCounts.Sum();
                    txtAnswerCount.text = value.ToString();
                }))
                .SetTarget(this);

        }

        public void Select()
        {
            Debug.Log($"Select: {answerIndex}");
            QuizManager.instance.SelectAnswer(answerIndex);
            SetState(QuizzAnswerButtonState.Selected);
        }

        public void SetState(QuizzAnswerButtonState state)
        {
            states.State = (int)state;
        }

        void DisableButtons(int index)
        {
            DisableButtons();
        }
        void DisableButtons()
        {
            btnSelect.enabled = false;
        }
    }
}
