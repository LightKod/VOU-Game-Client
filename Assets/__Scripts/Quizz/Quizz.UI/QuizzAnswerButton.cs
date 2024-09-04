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
        InCorrect = 3,
        Wrong = 4,
    }

    public class QuizzAnswerButton : MonoBehaviour
    {
        [ChildGameObjectsOnly][SerializeField] TextMeshProUGUI txtAnswer;
        [ChildGameObjectsOnly][SerializeField] TextMeshProUGUI txtAnswerCount;
        [ChildGameObjectsOnly][SerializeField] Slider sliderCorrect;
        [ChildGameObjectsOnly][SerializeField] Button btnSelect;
        [SerializeField] LeanSwitch states;
        string answer;
        private void Awake()
        {
            QuizManager.instance.onAnswerSelected += DisableButtons;
            QuizTimer.onCounterFinish += DisableButtons;
            btnSelect.onClick.AddListener(Select);
        }

        private void OnDestroy()
        {
            QuizManager.instance.onAnswerSelected -= DisableButtons;
            QuizTimer.onCounterFinish += DisableButtons;
        }

        [Button]
        public void SetDefaultState(string answer)
        {
            this.DOKill();
            this.answer = answer;
            txtAnswer.text = answer;
            sliderCorrect.value = 0;
            txtAnswerCount.text = "";
            SetState(QuizzAnswerButtonState.Default);
            btnSelect.enabled = true;
            btnSelect.targetGraphic.color = btnSelect.colors.normalColor;
        }

        public void SetResultState(string answer, AnswerObject answerObject, List<string> options)
        {
            this.DOKill();
            this.answer = answer;

            txtAnswer.text = answer;

            if (answer == answerObject.selectedAnswer)
            {
                if(answer == answerObject.correctAnswer)
                {
                    SetState(QuizzAnswerButtonState.Correct);
                }
                else
                {
                    SetState(QuizzAnswerButtonState.InCorrect);
                }
            }
            else
            {
                if(answer == answerObject.correctAnswer)
                {
                    SetState(QuizzAnswerButtonState.Correct);
                }
                else
                {
                    SetState(QuizzAnswerButtonState.Wrong);
                }
            }
            int sameAnswerCount = answerObject.answerCounts[options.IndexOf(answer)];
            if (sameAnswerCount == 0) return;
            Sequence animation = DOTween.Sequence()
                .AppendInterval(0.5f)
                .Append(DOVirtual.Int(0, sameAnswerCount, 1f, (int value) =>
                {
                    float sum = answerObject.answerCounts.Sum();
                    float nextValue = sum == 0 ? 0 : value * 1f / sum;
                    sliderCorrect.value = nextValue;
                    txtAnswerCount.text = value.ToString();
                }))
                .SetTarget(this);

        }

        public void Select()
        {
            Debug.Log($"Select: {answer}");
            QuizManager.instance.SelectAnswer(answer);
            SetState(QuizzAnswerButtonState.Selected);
        }

        public void SetState(QuizzAnswerButtonState state)
        {
            states.State = (int)state;
        }

        void DisableButtons(string answer)
        {
            DisableButtons();
        }
        void DisableButtons()
        {
            btnSelect.enabled = false;
        }
    }
}
