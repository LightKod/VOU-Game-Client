using Lean.Gui;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VOU
{
    public class QuizzWaitCountdown : MonoBehaviour
    {
        [SerializeField] LeanToggle toggle;
        [SerializeField] TextMeshProUGUI txtCountdown;

        bool isCounting;

        DateTime startTime;

        private void Update()
        {
            if (!isCounting) return;
            if(DateTime.Compare(DateTime.Now, startTime) < 0) 
            {
                txtCountdown.text = GetTimeDifference(DateTime.Now, startTime);
            }
            else
            {
                DisableUI();
            }
        }

        [Button]
        public void Test(int extraSecond)
        {
            DateTime testTime = DateTime.Now;
            testTime = testTime.AddSeconds(extraSecond);
            ShowUI(testTime);
        }

        public void ShowUI(DateTime startTime)
        {
            isCounting = true;
            this.startTime = startTime;
            toggle.TurnOn();
            txtCountdown.text = GetTimeDifference(DateTime.Now, startTime);
        }

        public void DisableUI()
        {
            isCounting = false;
            toggle.TurnOff();
        }


        public string GetTimeDifference(DateTime startTime, DateTime endTime)
        {
            // Calculate the time difference
            TimeSpan timeDifference = endTime - startTime;

            // Format the TimeSpan into hh:mm:ss
            string formattedDifference = string.Format("{0:D2}:{1:D2}:{2:D2}",
                timeDifference.Hours,
                timeDifference.Minutes,
                timeDifference.Seconds);

            return formattedDifference;
        }
    }
}
