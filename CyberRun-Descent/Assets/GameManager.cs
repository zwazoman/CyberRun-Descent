using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioClip[] _deathSound;
    [SerializeField] float _deathSoundVolume;

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

        SFXManager.Instance.PlaySFXClip(_deathSound,transform.position,_deathSoundVolume);
        PostProcessController.instance.FadeOut.play(true);
        //Time.timeScale = .5f;
        StartCoroutine(slowTimeDown());
        await Task.Delay(700);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator slowTimeDown()
    {
        print("--anim starting--");
        float t =  Time.unscaledTime;
        float endTime = t + .3f;
        while (t < endTime)
        {
            t = Time.unscaledTime;
            //print("putain;");
            float alpha = Mathf.InverseLerp(endTime - .3f, endTime, t);//1f-(endTime-Time.time)/duration;

            Time.timeScale = Mathf.Lerp(1, .4f, alpha);
            //parameter.value = (parameter.value*alpha);

            yield return null;
        }
        print("--anim done--");
    }
}
