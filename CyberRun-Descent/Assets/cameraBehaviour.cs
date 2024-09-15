using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehaviour : MonoBehaviour
{
    [Header("parametres")]
    [SerializeField]float PlayerAnticipationStrength = -1;
    [SerializeField] float PlayerAnticipationSmoothness;

    Vector3 basePose;
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
        offsetY = Mathf.SmoothDamp(offsetY, playerRB.velocity.y * PlayerAnticipationStrength, ref vel, PlayerAnticipationSmoothness);
        transform.position = ( offsetY ) * Vector3.up + basePose;
    }
}
