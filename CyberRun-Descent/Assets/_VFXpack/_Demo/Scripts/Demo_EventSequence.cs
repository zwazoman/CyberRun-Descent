using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Demo_EventSequence : MonoBehaviour
{
    [SerializeField] List<UnityEvent> events;
    [SerializeField] float delay;
    public void Play()
    {
        StartCoroutine(_play());
    }

    private void Start()
    {
        Play();
    }

    IEnumerator _play()
    {
        foreach(UnityEvent e in events)
        {
            if(e!=null) e.Invoke();
            yield return new WaitForSeconds(delay);
        }
    }
}
