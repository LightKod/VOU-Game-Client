using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Owlet
{
    public class RefreshRectTransform : MonoBehaviour
    {
        [SerializeField] RectTransform rect;
        [Button]
        public async void Refresh()
        {
            await Task.Yield(); //Wait 1 frame for others' update
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            
        }
    }
}
