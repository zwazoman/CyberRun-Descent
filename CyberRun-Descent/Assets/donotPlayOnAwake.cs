using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class donotPlayOnAwake : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        GetComponent<VisualEffect>().Stop();
    }
}
