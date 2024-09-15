using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class laserVisuals : MonoBehaviour
{
    MeshRenderer _meshRenderer;

    MaterialPropertyBlock _materialPropertyBlock;

    [SerializeField]
    float _blinkRate = 10;

    private void Start()
    {
        _materialPropertyBlock = new();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock,0);

        _materialPropertyBlock.SetFloat("_blinkRate", 0);
        _materialPropertyBlock.SetFloat("_staticAlpha", 0.02f);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock, 0);

    }

    public async Task startBlinking(float seconds)
    {
        print("tamere");
        _materialPropertyBlock.SetFloat("_blinkRate", 10);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock, 0);

        float endTime = Time.time + seconds;
        while (Time.time < endTime)
        {
            float alpha = 1f - (endTime - Time.time) / seconds;

            _materialPropertyBlock.SetFloat("_blinkRate", Mathf.Lerp(4,10,alpha));
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock, 0);

            await Task.Yield();
        }

    }

    public async Task shoot(float duration)
    {
        print("tamere shoot");

        _materialPropertyBlock.SetFloat("_blinkRate", 0);
        _materialPropertyBlock.SetFloat("_staticAlpha", 5);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock, 0);

        transform.localScale *= 1.5f;

        PostProcessController.instance.E_ExposureFlash.play();

        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            float alpha = 1f - (endTime - Time.time) / duration;

            _materialPropertyBlock.SetFloat("_staticAlpha", 1f-(alpha * alpha));
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock, 0);

            await Task.Yield();
        }
    }

    

}




