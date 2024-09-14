using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;
using System.Threading.Tasks;
using static PostProcessController;

public class PostProcessController : MonoBehaviour
{
    public VolumeProfile mVolumeProfile;
    public Vignette mVignette;
    public ColorAdjustments mColorAdjustments;
    public ExposureEffect mEffectAnimation;
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

        // get the intensity parameter and all other parameters to be used. 
        // PP parameters are usually an instance of ClampedParameter
        // don't create a new ClampedFloatParameter, get the reference to the existing one
        ClampedFloatParameter intensity = mVignette.intensity;
        intensity.value = 1f;

        FloatParameter exposure = mColorAdjustments.postExposure;
        exposure.value = 10;

    }

    [Serializable]
    public abstract class EffectAnimation<ComponentType,VolumeParameterType> where ComponentType : VolumeComponent where VolumeParameterType : VolumeParameter
    {
        public float duration;
        public AnimationCurve AlphaCurve01;

        protected ComponentType component;

        public void SetUp()
        {
            VolumeProfile mVolumeProfile = new();
            for (int i = 0; i < mVolumeProfile.components.Count; i++)
            {
                if (mVolumeProfile.components[i].GetType() == typeof( ComponentType))
                {
                    component = (ComponentType)mVolumeProfile.components[i];
                    print(component.name);
                }
            }
        }

        //@AUSECOURS comment on cancel une task?
        public async Task Play(VolumeParameterType parameter)
        {
            float endTime = Time.time + duration;
            while(Time.time < endTime)
            {
                float alpha = 1f-(endTime-Time.time)/duration;

                ApplyEffect(parameter, alpha);
                //parameter.value = (parameter.value*alpha);

                await Task.Yield();
            }
        }

        protected abstract void ApplyEffect(VolumeParameterType volumeParameter, float alpha);
    }

    [Serializable]
    public class ExposureEffect : EffectAnimation<ColorAdjustments, ClampedFloatParameter> 
    {
        public float from, to;
        protected override void ApplyEffect(ClampedFloatParameter volumeParameter, float alpha)
        {
            volumeParameter.value = Mathf.Lerp( from,to,alpha);
        }
    }

}