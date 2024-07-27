using Newtonsoft.Json;
using Owlet;
using Owlet.UI;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    //TODO: Upgrade this to use Socket
    public class QuizManager : Singleton<QuizManager>
    {
        QuizService service;

        public Action<QuestionObject> onQuestionReceive;
        public Action<QuestionObject, AnswerObject> onAnswerReceive;
        public Action<int> onAnswerSelected;
        public Action onQuestionTimerEnd;
        public Action onQuestionResultDisplayEnd;

        string roomID = "20";

        protected override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            SetupConnection();

            onQuestionReceive += DisplayQuestion;
            onAnswerReceive += DisplayResult;
            onQuestionTimerEnd += ClosePopup;
            onQuestionResultDisplayEnd += ClosePopup;
        }

        private void OnDestroy()
        {
            onQuestionReceive -= DisplayQuestion;
            onAnswerReceive -= DisplayResult;
            onQuestionTimerEnd -= ClosePopup;
            onQuestionResultDisplayEnd -= ClosePopup;

            service?.Dispose();
        }

        public async void SetupConnection()
        {
            service = new();
            await service.CreateConnection();
            service.JoinRoom(roomID);

            service.On(QuizService.EVENT_SEND_QUESTION, OnQuesionReceived);
            service.On(QuizService.EVENT_QUIZ_AUDIO, OnAudioReceived);
        }
        
        void OnQuesionReceived(SocketIOResponse res)
        {
            Debug.Log($"Question: {res}");
            string questionStr = res.GetValue<string>(0);
            QuestionObject questionObject = JsonConvert.DeserializeObject<QuestionObject>(questionStr);
            Debug.Log($"Question: {questionObject.question}");
        }

        void OnAudioReceived(SocketIOResponse res)
        {
            Debug.Log("Received Audio");
            //Debug.Log($"Audio: {res.GetValue<string>(0)}");
        }

        public void SelectAnswer(int index)
        {
            onAnswerSelected.Invoke(index);
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

        IEnumerator QuizzCycleTest()
        {
            TextAsset mockQuizzAsset = Resources.Load("QuizzMockData") as TextAsset;
            QuizzObject quizzObject = JsonConvert.DeserializeObject<QuizzObject>(mockQuizzAsset.text);

            yield return new WaitForSeconds(2f);
            int currentQuizz = 0;

            while(currentQuizz < quizzObject.questions.Count)
            {
                QuestionObject question = quizzObject.questions[currentQuizz];
                onQuestionReceive?.Invoke(question);

                yield return new WaitForSeconds(QuizzTimer.TIME_TO_ANSWER + 0.5f);//Wait for the timer

                onQuestionTimerEnd?.Invoke();

                yield return new WaitForSeconds(2f);//Wait for answer;
                AnswerObject answer = new AnswerObject();
                answer.resultIndex = UnityEngine.Random.Range(0, 3);
                answer.answerCounts = new int[]
                {
                    UnityEngine.Random.Range(100, 1500),
                    UnityEngine.Random.Range(100, 1500),
                    UnityEngine.Random.Range(100, 1500),
                };
                onAnswerReceive?.Invoke(question, answer);
                yield return new WaitForSeconds(5f);//Wait for random bullshit;
                onQuestionResultDisplayEnd?.Invoke();
                yield return new WaitForSeconds(2f);//Wait for random bullshit;

                currentQuizz++;
            }

            yield break;
        }
    }
}
