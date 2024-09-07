using DG.Tweening;
using Owlet;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace VOU
{
    public class Frieren_Talk : L2DModelAnimation
    {
        const string MOUTH_Y = "ParamMouthOpenY";

        [SerializeField] FloatRange mouthOpenRange;
        [SerializeField] FloatRange mouthCloseRange;
        [SerializeField] float mouthOpenTime = 0.1f;
        [SerializeField] float mouthCloseTime = 0.1f;

        Sequence talkSequence;
        protected override void Start()
        {
            base.Start();
            CreateTalkSequence();

            QuizManager.instance.onMCDataReceieve += StartTalking;
        }

        private void OnDestroy()
        {
            QuizManager.instance.onMCDataReceieve -= StartTalking;
        }

        void StartTalking(AudioClip audioClip)
        {
            StopAllCoroutines();
            StopTalking();

            Debug.Log($"Audio length: {audioClip.length}");
            StartCoroutine(TalkingCoroutine(audioClip.length));
        }

        IEnumerator TalkingCoroutine(float duration)
        {
            OpenMouth();
            yield return new WaitForSeconds(duration);
            StopTalking();
        }

        public override void UpdateModel()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                OpenMouth();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                StopTalking();
            }
        }


        void CreateTalkSequence()
        {
            talkSequence = DOTween.Sequence()
                .Append(DOVirtual.Float(parameters.GetParam(MOUTH_Y), mouthOpenRange.RandomValue(), mouthOpenTime,
                (float value) =>
                {
                    parameters.SetParam(MOUTH_Y, value);
                }))
                .Append(DOVirtual.Float(parameters.GetParam(MOUTH_Y), mouthCloseRange.RandomValue(), mouthCloseTime,
                (float value) =>
                {
                    parameters.SetParam(MOUTH_Y, value);
                }))
                .SetLoops(-1)
                .Pause()
                .SetTarget(this);
        }

        void OpenMouth()
        {
            DOVirtual.Float(parameters.GetParam(MOUTH_Y), mouthOpenRange.RandomValue(), mouthOpenTime,
                (float value) =>
                {
                    parameters.SetParam(MOUTH_Y, value);
                })
                .OnComplete(() => { CloseMouth(); })
                .SetTarget(this);
        }

        void CloseMouth()
        {
            DOVirtual.Float(parameters.GetParam(MOUTH_Y), mouthCloseRange.RandomValue(), mouthCloseTime,
                (float value) =>
                {
                    parameters.SetParam(MOUTH_Y, value);
                })
                .OnComplete(() => { OpenMouth(); })
                .SetTarget(this);
        }
        
        void StopTalking()
        {
            this.DOKill();

            DOVirtual.Float(parameters.GetParam(MOUTH_Y), 0, mouthCloseTime,
                (float value) =>
                {
                    parameters.SetParam(MOUTH_Y, value);
                });
        }
    }
}
