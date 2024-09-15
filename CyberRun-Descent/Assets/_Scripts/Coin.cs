
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Coin : MonoBehaviour
{
    [SerializeField] GameObject scorePopupPrefab;
    //private TMP_Text CoinTexte;
    bool isDead = false;

    [SerializeField] float magnetDistance = 3;
    GameObject player => Player.Instance.gameObject;

    Vector3 vel = new();



    private void Update()
    {
        Vector3 offset = player.transform.position - transform.position;
        if (Vector3.SqrMagnitude(offset) < magnetDistance * magnetDistance )
        {
            magnetDistance = 100;
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref vel, 0.07f,10);
        }

        if (!isDead)
        {
            transform.Rotate(Vector3.up * 180 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject && !isDead)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        //AudioManager.Instance.PlayCoin();
        
        transform.localScale *= 1.6f;

        GameObject Popup = GameObject.Instantiate(scorePopupPrefab, transform.position + Vector3.forward * -1, Quaternion.identity);
        Popup.GetComponent<scorePopup>().init(200, Color.white);
        //Popup.transform.localScale *= 2;

        yield return new WaitForSeconds(0.08f);
        //Debug.Log("Tu as " + nmbreDePiece + " Coins");
        Destroy(gameObject);
    }
}


