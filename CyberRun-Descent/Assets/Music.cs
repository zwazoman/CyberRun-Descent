using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Music : MonoBehaviour
{
    public bool EnableDontDestroyOnLoad = false;
    static Music instance;

    private void Awake()
    {
        if (!EnableDontDestroyOnLoad) return;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

}
