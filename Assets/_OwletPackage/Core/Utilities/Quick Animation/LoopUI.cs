using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Owlet.Animations
{
    public class LoopUI : MonoBehaviour
    {
        public float interval = 0.3f;
        public List<Sprite> sprites;
        float current;
        int index = 0;
        Image render;

        // Start is called before the first frame update
        void Start()
        {
            current = Time.time + interval;
            render = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.unscaledTime > current)
            {
                render.sprite = sprites[index++];
                if (index >= sprites.Count)
                {
                    index = 0;
                }
                current = Time.unscaledTime + interval;
            }
        }
    }
}