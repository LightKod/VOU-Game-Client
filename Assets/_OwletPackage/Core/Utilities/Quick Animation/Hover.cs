using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet.Animations
{
    public class Hover : MonoBehaviour
    {
        public float amplitude = 1.0f;
        public float frequency = 1.0f;


        void Update()
        {
            float y = amplitude * Mathf.Sin(Time.time * frequency);
            transform.localPosition = new Vector3(0, y, 0);
        }
    }
}
