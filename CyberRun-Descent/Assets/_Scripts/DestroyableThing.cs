using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableThing : MonoBehaviour
{
    public void hit()
    {
        Destroy(gameObject);
    }
}
