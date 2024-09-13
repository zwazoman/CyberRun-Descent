using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

namespace SimpleVFXs
{
    public class Demo_FakeProjectile : MonoBehaviour
    {

        [SerializeField] Transform _target;
        [SerializeField] Transform _startTransform;
        [SerializeField] Transform _endTransform;
        [SerializeField] float _duration = .5f;
        [SerializeField] UnityEvent _OnAnimationEnd;

        public void PlayAnimation() => StartCoroutine(_playAnimation());

        private void Start()
        {
            _target.gameObject.SetActive(false);
        }

        IEnumerator _playAnimation()
        {
            float endTime = Time.time + _duration;

            _target.gameObject.SetActive(true);
            _target.position = Vector3.Lerp(_startTransform.position, _endTransform.position, 0f);

            while (Time.time < endTime)
            {
                float alpha = 1f - (endTime - Time.time) / _duration;

                _target.position = Vector3.Lerp(_startTransform.position, _endTransform.position, alpha);
                
                yield return null;
            }

            _target.gameObject.SetActive(false);
            _target.position = Vector3.Lerp(_startTransform.position, _endTransform.position, 1f);
            _OnAnimationEnd.Invoke();
        }

    }
}

