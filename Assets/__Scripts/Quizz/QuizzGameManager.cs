using Owlet;
using Owlet.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class QuizzGameManager : Singleton<QuizzGameManager>
    {
        private void Start()
        {
            QuizzService.instance.onQuestionReceive += DisplayQuestion;
            QuizzService.instance.onAnswerReceive += DisplayResult;
            QuizzService.instance.onQuestionTimerEnd += ClosePopup;
            QuizzService.instance.onQuestionResultDisplayEnd += ClosePopup;
        }

        private void OnDestroy()
        {
            QuizzService.instance.onQuestionReceive -= DisplayQuestion;
            QuizzService.instance.onAnswerReceive -= DisplayResult;
            QuizzService.instance.onQuestionTimerEnd -= ClosePopup;
            QuizzService.instance.onQuestionResultDisplayEnd -= ClosePopup;
        }

        private void ClosePopup()
        {
            PopupManager.instance.CloseUI(Keys.Popup.QuizzAnswerSelector);
        }

        async void DisplayQuestion(QuestionObject questionObject)
        {
            QuizzAnswerSelector answerSelector = await PopupManager.instance.OpenUI<QuizzAnswerSelector>(Keys.Popup.QuizzAnswerSelector, 1);
            answerSelector.SetupUI(questionObject);
        }

        async void DisplayResult(QuestionObject questionObject, AnswerObject answerObject)
        {
            QuizzAnswerSelector answerSelector = await PopupManager.instance.OpenUI<QuizzAnswerSelector>(Keys.Popup.QuizzAnswerSelector, 1);
            answerSelector.SetupUIResult(questionObject, answerObject);
        }

    }
}
