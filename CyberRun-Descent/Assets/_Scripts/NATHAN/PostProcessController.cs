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
    public FadingEffect FadeIn = new();
    public FadingEffect FadeOut = new();

    //singleton
    public static PostProcessController instance { get; set; }

    public Dictionary<Type, Coroutine> effectsCoroutines = new();

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        E_ExposureFlash.SetUp(mVolumeProfile);
        E_ExposureFlashLong.SetUp(mVolumeProfile);
        E_ScreenDistortion.SetUp(mVolumeProfile);
        FadeIn.SetUp(mVolumeProfile);
        FadeOut.SetUp(mVolumeProfile);

        FadeIn.play(true);
    }

    private void OnDestroy()
    {
        E_ExposureFlash.OnDestroy();
        E_ExposureFlashLong.OnDestroy();
        E_ScreenDistortion.OnDestroy();
        FadeIn.OnDestroy();
        FadeOut.OnDestroy();
    }



    //-----------------------------  classe abstraite  -----------------------------
    [Serializable]
    public abstract class PostProcessEffectAnimation<ComponentType> where ComponentType : VolumeComponent
    {
        public float duration;

        protected ComponentType _component;
        
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

            if (!PostProcessController.instance.effectsCoroutines.ContainsKey(this.GetType()))
            {
                PostProcessController.instance.effectsCoroutines.Add(this.GetType(), null);
            }

            OnSetUp();
            //print("--- Set Up End ---");

        }

        protected abstract void OnSetUp();

        public abstract void OnDestroy();

        public void play(bool useUnscaledTime = false)
        {
            stop(PostProcessController.instance);
            PostProcessController.instance.effectsCoroutines[this.GetType()] = PostProcessController.instance.StartCoroutine(_Play(useUnscaledTime));
        }
        public void stop(MonoBehaviour mb)
        {
            if (PostProcessController.instance.effectsCoroutines[this.GetType()] != null)
            {
                print("STOOOOOOOOOOOOOOOOP");
                mb.StopCoroutine(PostProcessController.instance.effectsCoroutines[this.GetType()]);
            }
            PostProcessController.instance.effectsCoroutines[this.GetType()] = null;
        }
        private IEnumerator _Play(bool useUnscaledTime = false)
        {
            //print("--anim starting--");
            float t = (useUnscaledTime ? Time.unscaledTime : Time.time);
            float endTime = t + duration;
            OnBeforePlay();
            while (t < endTime)
            {
                t = (useUnscaledTime ? Time.unscaledTime : Time.time);
                //print("putain;");
                float alpha = Mathf.InverseLerp(endTime - duration, endTime, t);//1f-(endTime-Time.time)/duration;
                //print("Alpha:" + alpha);
                ApplyEffect(_component, alpha);
                //parameter.value = (parameter.value*alpha);

                yield return null;
            }
            ApplyEffect(_component,1);
           // print("--anim done--");
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
            try { _component.postExposure.value = startValue; } catch { };
        }

        protected override void ApplyEffect(ColorAdjustments component, float alpha)
        {
            alpha = AlphaCurve01.Evaluate(alpha);
            float v = startValue;//Mathf.Lerp(AnimStartValue, startValue, alpha);
            component.postExposure.value = Mathf.Lerp(v,v + offset, alpha);
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
            try{ _component.intensity.value = startValue; } catch { };
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

    [Serializable]
    public class FadingEffect : PostProcessEffectAnimation<ColorAdjustments> 
    {
        Color startValue;

        Color AnimStartValue;

        public AnimationCurve AlphaCurve01;
        public override void OnDestroy()
        {
            try{_component.colorFilter.value = startValue;} catch { };
        }

        protected override void ApplyEffect(ColorAdjustments component, float alpha)
        {
            alpha = AlphaCurve01.Evaluate(alpha);
            //Color v = Color.Lerp(AnimStartValue, startValue, alpha);
            component.colorFilter.value = Color.Lerp(AnimStartValue,Color.black, alpha);
        }

        protected override void OnBeforePlay()
        {
            AnimStartValue = _component.colorFilter.value;
        }

        protected override void OnSetUp()
        {
            startValue = _component.colorFilter.value;
        }

    }

}