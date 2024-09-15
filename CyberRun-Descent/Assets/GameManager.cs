using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public static float Difficulty = 1;
    
    const float MaxDificulty = 3;
    const float DifficultyRampUpDuration = 60;

    public event Action OnGameOver;

    private void Awake()
    {
        instance = this;
        Difficulty = 1;
        Time.timeScale = 1;
    }

    private void Update()
    {
        Difficulty = Mathf.Lerp(1,Difficulty,Time.time/ DifficultyRampUpDuration);
    }

    public async void TriggerGameOver()
    {
        OnGameOver?.Invoke();
        Time.timeScale = .5f;
        await Task.Delay(2000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
