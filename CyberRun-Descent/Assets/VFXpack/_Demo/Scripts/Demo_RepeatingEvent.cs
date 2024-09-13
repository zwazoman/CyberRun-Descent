using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Demo_RepeatingEvent : MonoBehaviour
{
    [SerializeField] float _delay = 1;
    [SerializeField] UnityEvent _unityEvent;

    private void Start()
    {
        InvokeRepeating("invokeEvent", _delay, _delay);
    }
    void invokeEvent()
    {
        _unityEvent.Invoke();
    }
}
