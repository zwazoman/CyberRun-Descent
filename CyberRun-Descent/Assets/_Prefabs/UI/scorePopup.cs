using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scorePopup : MonoBehaviour
{
    
    public void setText(string txt, float animSpeed=1)
    {
        //GetComponentInChildren<Animation>(). = animSpeed;
        GetComponentInChildren<TMP_Text>().text=txt;
    }
    // Update is called once per frame
    public void init(int score, Color color )
    {
        //float s = Random.Range(0.8f, 1.2f) * 0.5f;
        //GetComponentInChildren<Animator>().speed = s;
        Destroy(gameObject, 0.4f);
        transform.localScale *= 0.15f;

        TMP_Text text = GetComponentInChildren<TMP_Text>();
        
        //text.outlineColor = color;
        text.text = "+" + score.ToString();
        
        ScoreManager.Instance.IncreaseScore( score); 
    }
}
