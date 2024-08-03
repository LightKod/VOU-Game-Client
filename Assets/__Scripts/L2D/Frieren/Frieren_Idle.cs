using DG.Tweening;
using Owlet;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class Frieren_Idle : L2DModelAnimation
    {
        const string BREATH = "ParamBreath";
        const string STAFF = "Param56";
        [Header("Breath")]
        [SerializeField] float breathTime = 1.5f;
        [Header("Staff")]
        public FloatRange staffAnimationInveral;

        float staffAnimationCounter;

        Sequence breathSequence;
        Sequence staffSequence;
        Sequence earTwitchSequence;

        protected override void Start()
        {
            base.Start();
            CreateBreathSequence();
            CreateStaffSequence();
            staffAnimationCounter = staffAnimationInveral.RandomValue();
        }

        public override void UpdateModel()
        {
            staffAnimationCounter -= Time.deltaTime;
            if(staffAnimationCounter <= 0)
            {
                staffAnimationCounter = staffAnimationInveral.RandomValue();
                staffSequence.Restart();
            }
        }

        void CreateBreathSequence()
        {
            breathSequence = DOTween.Sequence()
                .Append(DOVirtual.Float(0f, 1f, 1.5f, (float value) =>
                {
                    parameters.SetParam(BREATH, value);
                }))
                .Append(DOVirtual.Float(1f, 0f, 2.5f, (float value) =>
                {
                    parameters.SetParam(BREATH, value);
                }))
                .SetLoops(-1)
                .SetTarget(this);
        }

        void CreateStaffSequence()
        {
            staffSequence = DOTween.Sequence()
                .Append(DOVirtual.Float(0f, 15f, 6f, (float value) =>
                {
                    parameters.SetParam(STAFF, value);
                }).SetEase(Ease.OutBack))
                .Append(DOVirtual.Float(15f, 0f, 2f, (float value) =>
                {
                    parameters.SetParam(STAFF, value);
                }))
                .Pause()
                .SetTarget(this);

        }
    }
}
