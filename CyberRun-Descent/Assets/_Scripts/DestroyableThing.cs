using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableThing : MonoBehaviour
{
    [SerializeField]scorePopup scorePopupPrefab;
    public void hit()
    {
        Vector3 center = GetComponent<Collider>().bounds.center;
        scorePopup popup = GameObject.Instantiate(scorePopupPrefab, center, Quaternion.identity);
        popup.init(100, Color.white);

        Destroy(gameObject);
    }
}
