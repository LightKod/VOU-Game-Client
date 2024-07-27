using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    [System.Serializable]
    public class QuestionObject
    {
        public string question;
        public List<string> options;
        public float timeLimit;
    }
}
