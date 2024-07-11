using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet.Animations
{
    public class Breeth : MonoBehaviour
    {

        [SerializeField] float breethIn = 0.95f;
        [SerializeField] float breethOut = 1.05f;
        [SerializeField] float interval = 1f;

        private float timer = 0f;
        private bool isBreathingIn = true;

        private void Update()
        {
            timer += Time.deltaTime;

            if (isBreathingIn)
            {
                float progress = timer / interval;
                transform.localScale = Vector3.Lerp(Vector3.one * breethIn, Vector3.one * breethOut, progress);
                if (timer >= interval)
                {
                    // Switch to breathing out
                    isBreathingIn = false;
                    timer = 0f;
                }
            }
            else
            {
                float progress = timer / interval;
                transform.localScale = Vector3.Lerp(Vector3.one * breethOut, Vector3.one * breethIn, progress);
                if (timer >= interval)
                {
                    // Switch to breathing in
                    isBreathingIn = true;
                    timer = 0f;
                }
            }
        }
    }
}
