using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet
{
    [System.Serializable]
    public class FloatRange
    {
        [HorizontalGroup]
        public float min;
        [HorizontalGroup]
        public float max;

        public float RandomValue()
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    [System.Serializable]
    public class IntRange
    {
        [HorizontalGroup]
        public int min;
        [HorizontalGroup]
        public int max;

        public int RandomValue()
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
    }
}
