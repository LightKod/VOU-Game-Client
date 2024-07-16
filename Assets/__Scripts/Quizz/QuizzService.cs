using Newtonsoft.Json;
using Owlet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    //TODO: Upgrade this to use Socket
    public class QuizzService : Singleton<QuizzService>
    {
        //Trying to replicate the Socket behavior...

        public Action<QuestionObject> onQuestionReceive;
        public Action<QuestionObject, AnswerObject> onAnswerReceive;  //Send index
        public Action<int> onAnswerSelected;
        public Action onQuestionTimerEnd;  //Call when the server emit
        public Action onQuestionResultDisplayEnd;  //Call when the server emit

        protected override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            StartCoroutine(QuizzCycleTest());
        }

        private void OnDestroy()
        {
            
        }

        public void SetupConnection()
        {
            //Setup socket connection
        }

        public void SelectAnswer(int index)
        {
            onAnswerSelected.Invoke(index);
            //Send this shit to the server
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
                answer.isCorrect = true;
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
