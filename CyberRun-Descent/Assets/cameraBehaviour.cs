using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehaviour : MonoBehaviour
{
    [Header("parametres")]
    [SerializeField]float PlayerAnticipationStrength = -1;
    [SerializeField] float PlayerAnticipationSmoothness;
    [SerializeField] float positionWeight;

    public Vector3 basePose;
    float offsetY;

    Rigidbody playerRB;

    private void Awake()
    {
        basePose = transform.position;
    }

    private void Start()
    {
        playerRB = Player.Instance.RB;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float vel = 0;
        offsetY = Mathf.SmoothDamp(offsetY, playerRB.velocity.y * PlayerAnticipationStrength + (basePose.y- playerRB.position.y) * positionWeight, ref vel, PlayerAnticipationSmoothness,Mathf.Infinity);
        transform.position = ( offsetY ) * Vector3.up + basePose;
    }

    public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
    {
        smoothTime = Mathf.Max(0.0001f, smoothTime);
        float num = 2f / smoothTime;
        float num2 = num * Time.deltaTime;
        float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
        float value = current - target;
        float num4 = target;
        float num5 = maxSpeed * smoothTime;
        value = Mathf.Clamp(value, 0f - num5, num5);
        target = current - value;
        float num6 = (currentVelocity + num * value) * Time.deltaTime;
        currentVelocity = (currentVelocity - num * num6) * num3;
        float num7 = target + (value + num6) * num3;


        return num7;
    }
}
