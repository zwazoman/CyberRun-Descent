using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    //Singleton
    private static ScoreManager _instance = null;
    public static ScoreManager Instance => _instance;

    public int score {  get ; private set;}  = 0;

    [Header("Display")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text HighScoreText;
    private void Awake()
    {
        //Singleton
        if (_instance != null && _instance != this)
        {
            _instance.score = 0;
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        updateScoreDisplay();
    }

    public void IncreaseScore(int toAdd)
    {
        score += toAdd;

#if UNITY_EDITOR
        if(PlayerPrefs.GetInt("EDITOR_HighScore"+SceneManager.GetActiveScene().name) < score)
        {
            PlayerPrefs.SetInt("EDITOR_HighScore" + SceneManager.GetActiveScene().name, score) ;
        }
#else
        if(PlayerPrefs.GetInt("HighScore"+SceneManager.GetActiveScene().name) < score)
        {
            PlayerPrefs.SetInt("HighScore" + SceneManager.GetActiveScene().name, score) ;
        }
#endif
        updateScoreDisplay();
    }

    void updateScoreDisplay()
    {
        scoreText.text = "Score: " + score.ToString();


#if UNITY_EDITOR
        if (HighScoreText != null) HighScoreText.text = "Best: " + PlayerPrefs.GetInt("EDITOR_HighScore" + SceneManager.GetActiveScene().name).ToString();
#else
        if (HighScoreText != null) HighScoreText.text = "Best: " + PlayerPrefs.GetInt("HighScore" + SceneManager.GetActiveScene().name).ToString();
#endif

    }

}
