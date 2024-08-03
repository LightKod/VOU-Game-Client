using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class ExtractInstanceID : MonoBehaviour
    {
        [Button]
        void PrintInstanceID(ScriptableObject clip)
        {
            Debug.Log(clip.GetInstanceID());
        }
    }
}
