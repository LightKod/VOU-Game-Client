using Lean.Gui;
using Lean.Transition;
using Lean.Transition.Method;
using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class QuizzAnswerSelector : Popup
    {
        [SerializeField] TextMeshProUGUI txtQuestion;
        [SerializeField] List<QuizzAnswerButton> buttons;
        [Header("Timer")]
        [SerializeField] GameObject timer;
        [SerializeField] Slider sliderTimer;
        [SerializeField] Image imgTimerFill;
        [SerializeField] TextMeshProUGUI txtTimer;
        [SerializeField] LeanPlayer pulseTimer;

        float previousIntCounter = 0;

        protected override void OnEnableUI()
        {
            QuizzTimer.onCounterTick += UpdateTimer;
            base.OnEnableUI();
        }

        protected override void OnDisableUI()
        {
            QuizzTimer.onCounterTick -= UpdateTimer;
            base.OnDisableUI();
        }

        public void SetupUI(QuestionObject questionObject)
        {
            //TODO: Text appear animation
            timer.SetActive(true);
            txtQuestion.text = questionObject.question;
            ColorUtility.TryParseHtmlString(Keys.Color.CoralBlue, out Color color);
            imgTimerFill.color = color;
            txtTimer.text = ((int)QuizzTimer.TIME_TO_ANSWER).ToString();


            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].SetDefaultState(i, questionObject.answers[i]);
            }
        }

        public void SetupUIResult(QuestionObject questionObject, AnswerObject answerObject)
        {
            timer.SetActive(false);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].SetResultState(i, questionObject.answers[i], answerObject);
            }
        }

        void UpdateTimer(float timeLeft)
        {
            int timerLeftInt = (int) timeLeft;
            if (previousIntCounter != timerLeftInt)
            {
                previousIntCounter = timerLeftInt;
                Color color;
                if(previousIntCounter <= 3)
                {
                    ColorUtility.TryParseHtmlString(Keys.Color.Red, out color);
                    pulseTimer.Begin();
                }
                else
                {
                    ColorUtility.TryParseHtmlString(Keys.Color.CoralBlue, out color);
                }
                imgTimerFill.color = color;
            }
            txtTimer.text = ((int)timeLeft).ToString();
            sliderTimer.value = (QuizzTimer.TIME_TO_ANSWER - timeLeft) / QuizzTimer.TIME_TO_ANSWER;
        }
    }
}
