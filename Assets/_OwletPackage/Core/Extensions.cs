using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Owlet
{
    public static class Extensions
    {
        public static void ValueOverTime(this Slider slider, float toValue, float time)
        {

        }

        public static List<T> GetUniqueRandomElements<T>(List<T> inputList, int count)
        {
            List<T> inputListClone = new List<T>(inputList);
            Shuffle(inputListClone);
            count = Mathf.Min(count, inputList.Count);
            return inputListClone.GetRange(0, count);
        }

        public static void Shuffle<T>(List<T> inputList)
        {
            for (int i = 0; i < inputList.Count - 1; i++)
            {
                T temp = inputList[i];
                int rand = Random.Range(i, inputList.Count);
                inputList[i] = inputList[rand];
                inputList[rand] = temp;
            }
        }

        public static T GetRandom<T>(List<T> inputList)
        {
            return inputList[Random.Range(0, inputList.Count - 1)];
        }

        /// <summary>
        /// Invoke every coroutine
        /// </summary>
        /// <param name="events"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerator InvokeOnState(System.MulticastDelegate events, params object[] args)
        {
            if (events != null)
            {
                foreach (var @delegate in events.GetInvocationList())
                {
                    yield return @delegate.DynamicInvoke(args);
                }
            }
        }


        /// <summary>
        /// Invoke the last event added in
        /// </summary>
        /// <param name="events"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerator InvokeStack(System.MulticastDelegate events, params object[] args)
        {
            if (events != null)
            {
                var delegates = events.GetInvocationList();
                var del = delegates[delegates.Length - 1];
                yield return del.DynamicInvoke(args);
            }
        }

        public static Vector3 CartToIso(Vector3Int coord, Vector3 offset)
        {
            float isoX = (coord.x - coord.y) / 2f;
            float isoY = (coord.x + coord.y + 2) / 4f + (coord.z + 1) / 4f;
            return new Vector3(isoX, isoY) + offset;
        }

        public static Vector3 WorldToCanvasPosition(this Canvas canvas, Vector3 worldPosition, Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            var viewportPosition = camera.WorldToViewportPoint(worldPosition);
            return canvas.ViewportToCanvasPosition(viewportPosition);
        }

        public static Vector3 ScreenToCanvasPosition(this Canvas canvas, Vector3 screenPosition)
        {
            var viewportPosition = new Vector3(screenPosition.x / Screen.width,
                                               screenPosition.y / Screen.height,
                                               0);
            return canvas.ViewportToCanvasPosition(viewportPosition);
        }

        public static Vector3 ViewportToCanvasPosition(this Canvas canvas, Vector3 viewportPosition)
        {
            var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
            var canvasRect = canvas.GetComponent<RectTransform>();
            var scale = canvasRect.sizeDelta;
            return Vector3.Scale(centerBasedViewPortPosition, scale);
        }
    }
}