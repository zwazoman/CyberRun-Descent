using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scorePopup : MonoBehaviour
{
    Transform cam;

    public void setText(string txt, float animSpeed=1)
    {
        //GetComponentInChildren<Animation>(). = animSpeed;
        GetComponentInChildren<TMP_Text>().text=txt;
    }
    // Update is called once per frame
    public void init(int score, Color color )
    {
        cam = Camera.main.gameObject.transform;
        transform.localScale = new Vector3(-1, 1, 1);
        //float s = Random.Range(0.8f, 1.2f) * 0.5f;
        //GetComponentInChildren<Animator>().speed = s;
        Destroy(gameObject, 0.4f);
        transform.localScale *= 0.3f;

        TMP_Text text = GetComponentInChildren<TMP_Text>();
        
        //text.outlineColor = color;
        text.text = "+" + score.ToString();
        
        ScoreManager.Instance.IncreaseScore( score); 
    }

    private void Update()
    {
        transform.LookAt(cam.position, Vector3.up) ;
    }
}
