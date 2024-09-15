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
    public VolumeProfile mVolumeProfile;
    public Vignette mVignette;
    public ColorAdjustments mColorAdjustments;


    public ExposureEffect _exposureEffect;

    void Start()
    {
        // get the effects
        for (int i = 0; i < mVolumeProfile.components.Count; i++)
        {
            if (mVolumeProfile.components[i].name == "Vignette")
            {
                mVignette = (Vignette)mVolumeProfile.components[i];
                print("a");
            }

            if (mVolumeProfile.components[i].name == "ColorAdjustments")
            {
                mColorAdjustments = (ColorAdjustments)mVolumeProfile.components[i];
                print("b");

            }
        }

        _exposureEffect.SetUp();

        InvokeRepeating("t", 2, 2);
    }

    private void t()
    {
        _exposureEffect.play(this);
    }

    private void OnDestroy()
    {
        _exposureEffect.OnDestroy(this);
    }

    [Serializable]
    public abstract class EffectAnimation<ComponentType> where ComponentType : VolumeComponent
    {
        public float duration;
        public AnimationCurve AlphaCurve01;

        protected ComponentType _component;
        
        private Coroutine activeCoroutine;

        public void SetUp()
        {
            VolumeProfile mVolumeProfile = new();
            for (int i = 0; i < mVolumeProfile.components.Count; i++)
            {
                if (mVolumeProfile.components[i].GetType() == typeof( ComponentType))
                {
                    _component = (ComponentType)mVolumeProfile.components[i];
                    print(_component.name);
                }
            }
        }

        protected abstract void OnSetUp();


        public abstract void OnDestroy(MonoBehaviour mb);

        public void play(MonoBehaviour mb)
        {
            activeCoroutine = mb.StartCoroutine(_Play());
        }
        public void stop(MonoBehaviour mb)
        {
            if(activeCoroutine!=null) mb.StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
        private IEnumerator _Play()
        {
            float endTime = Time.time + duration;
            while(Time.time < endTime)
            {
                float alpha = 1f-(endTime-Time.time)/duration;

                ApplyEffect(_component, alpha);
                //parameter.value = (parameter.value*alpha);

                yield return null;
            }
        }

        protected abstract void ApplyEffect(ComponentType component, float alpha);
    }

    [Serializable]
    public class ExposureEffect : EffectAnimation<ColorAdjustments> 
    {
        public float from, to;
        float startValue;

        public override void OnDestroy(MonoBehaviour mb)
        {
            stop(mb);
            _component.postExposure.value = startValue;
        }

        protected override void ApplyEffect(ColorAdjustments component, float alpha)
        {
            component.postExposure.value = Mathf.Lerp( from,to,alpha);
        }

        protected override void OnSetUp()
        {
            startValue = _component.postExposure.value;
        }

    }

}