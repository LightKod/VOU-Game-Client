using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet
{
    public class PushMeshToFront : MonoBehaviour
    {
        string layerToPushTo = "Text";
        void Awake()
        {
            GetComponent<Renderer>().sortingLayerName = layerToPushTo;
        }
    }
}
