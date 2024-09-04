using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class QuizTimer : MonoBehaviour
    {
        public static float TIME_TO_ANSWER = 15;
        float counter;

        bool isCounting = false;

        public static Action<float> onCounterTick;
        public static Action onCounterFinish;

        private void Start()
        {
            QuizManager.instance.onQuestionReceive += StartCounter;
        }

        private void OnDestroy()
        {
            QuizManager.instance.onQuestionReceive -= StartCounter;
        }

        public void StartCounter(QuestionObject questionObject)
        {
            isCounting = true;
            counter = TIME_TO_ANSWER = questionObject.timeLimit;
        }

        public void StopCounter()
        {
            isCounting = false;
        }


        private void Update()
        {
            if (!isCounting) return;
            counter -= Time.deltaTime;
            onCounterTick?.Invoke(counter);
            if (counter <= 0)
            {
                counter = 0;
                onCounterFinish?.Invoke();
                isCounting = false;
            }
        }

    }
}
