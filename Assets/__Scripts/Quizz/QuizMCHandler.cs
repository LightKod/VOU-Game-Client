using Owlet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class QuizMCHandler : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;


        private void Start()
        {
            QuizManager.instance.onMCDataReceieve += SetAudioClip;
        }

        private void OnDestroy()
        {
            QuizManager.instance.onMCDataReceieve -= SetAudioClip;
        }

        void SetAudioClip(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
