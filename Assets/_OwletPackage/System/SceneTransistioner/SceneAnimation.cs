using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Owlet.Systems.SceneTransistions
{
    [CreateAssetMenu(menuName ="Owlet/Systems/Scene Transistion/Default")]
    public class SceneAnimation : ScriptableObject
    {
        public virtual void Animation(bool hideGameplayScene, CanvasGroup background)
        {
            background.DOComplete();
            float target = hideGameplayScene ? 1 : 0;
            background.DOFade(target, 0.5f);
        }
    }
}
