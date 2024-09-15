using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;
using System.Threading.Tasks;
using static PostProcessController;
using Unity.VisualScripting;
using System.ComponentModel;

public class PostProcessController : MonoBehaviour
{
    [Header("references")]
    [SerializeField] VolumeProfile mVolumeProfile;

    [Header("Effects")]
    public ExposureEffect E_ExposureFlash = new();
    public ScreenDistortionEffect E_ScreenDistortion = new();
    public ExposureEffect E_ExposureFlashLong = new();

    public static PostProcessController instance { get; set; }

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        E_ExposureFlash.SetUp(mVolumeProfile);
        E_ExposureFlashLong.SetUp(mVolumeProfile);
        E_ScreenDistortion.SetUp(mVolumeProfile);
    }

    private void OnDestroy()
    {
        E_ExposureFlash.OnDestroy();
        E_ExposureFlashLong.OnDestroy();
        E_ScreenDistortion.OnDestroy();
    }



    //-----------------------------  classe abstraite  -----------------------------
    [Serializable]
    public abstract class PostProcessEffectAnimation<ComponentType> where ComponentType : VolumeComponent
    {
        public float duration;

        protected ComponentType _component;
        
        private Coroutine activeCoroutine;

        public void SetUp(VolumeProfile mVolumeProfile)
        {
            //print("--- Set Up ---");
            for (int i = 0; i < mVolumeProfile.components.Count; i++)
            {
                //print(mVolumeProfile.components[i]);

                if (mVolumeProfile.components[i].GetType() == typeof( ComponentType))
                {
                    _component = (ComponentType)mVolumeProfile.components[i];
                    print(_component.name);
                }
            }
            //print("--- Set Up End ---");

        }

        protected abstract void OnSetUp();

        public abstract void OnDestroy();

        public void play()
        {
            stop(PostProcessController.instance);
            activeCoroutine = PostProcessController.instance.StartCoroutine(_Play());
        }
        public void stop(MonoBehaviour mb)
        {
            if(activeCoroutine!=null) mb.StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
        private IEnumerator _Play()
        {
           // print("--anim starting--");

            float endTime = Time.time + duration;
            OnBeforePlay();
            while (Time.time < endTime)
            {
                //print("putain;");
                float alpha = Mathf.InverseLerp(endTime - duration, endTime, Time.time);//1f-(endTime-Time.time)/duration;
                //print("Alpha:" + alpha);
                ApplyEffect(_component, alpha);
                //parameter.value = (parameter.value*alpha);

                yield return null;
            }
            //print("--anim done--");
        }

        protected virtual void OnBeforePlay()
        {

        }

        protected abstract void ApplyEffect(ComponentType component, float alpha);
    }


    //-----------------------------  classes concretes  -----------------------------
    [Serializable]
    public class ExposureEffect : PostProcessEffectAnimation<ColorAdjustments> 
    {
        public float offset;
        float startValue;

        float AnimStartValue;

        public AnimationCurve AlphaCurve01;
        public override void OnDestroy()
        {
            _component.postExposure.value = startValue;
        }

        protected override void ApplyEffect(ColorAdjustments component, float alpha)
        {
            alpha = AlphaCurve01.Evaluate(alpha);
            component.postExposure.value = Mathf.Lerp(Mathf.Lerp(AnimStartValue, startValue,alpha), startValue + offset, alpha);
        }

        protected override void OnBeforePlay()
        {
            AnimStartValue = _component.postExposure.value;
        }

        protected override void OnSetUp()
        {
            startValue = _component.postExposure.value;
        }

    }

    [Serializable]
    public class ScreenDistortionEffect : PostProcessEffectAnimation<LensDistortion>
    {
        public float offset;
        float startValue;

        float AnimStartValue;

        public AnimationCurve AlphaCurve01;
        public override void OnDestroy()
        {
            _component.intensity.value = startValue;
        }

        protected override void ApplyEffect(LensDistortion component, float alpha)
        {
            alpha = AlphaCurve01.Evaluate(alpha);
            component.intensity.value = Mathf.Lerp(Mathf.Lerp(AnimStartValue, startValue, alpha), startValue + offset, alpha);
        }

        protected override void OnBeforePlay()
        {
            AnimStartValue = _component.intensity.value;
        }

        protected override void OnSetUp()
        {
            startValue = _component.intensity.value;
        }

    }


}