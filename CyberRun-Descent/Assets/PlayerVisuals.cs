using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    public void flashChromaticAberration()
    {
        PostProcessController.instance.E_ScreenDistortion.play();
        PostProcessController.instance.E_ExposureFlashLong.play();
        print("Salope");

    }
}
