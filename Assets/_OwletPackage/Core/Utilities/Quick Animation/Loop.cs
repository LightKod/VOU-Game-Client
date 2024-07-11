using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet.Animations
{
    public class Loop : MonoBehaviour
    {
        public float interval = 0.3f;
        public bool loop = true;
        public List<Sprite> sprites;
        float current;
        int index = 0;
        SpriteRenderer render;
        bool stop = false;

        // Start is called before the first frame update
        void Start()
        {
            current = Time.time + interval;
            render = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (stop) return;
            if (Time.time > current)
            {
                render.sprite = sprites[index++];
                if (index >= sprites.Count)
                {
                    index = 0;
                    if (!loop) stop = true;
                }
                current = Time.time + interval;
            }
        }
    }

}