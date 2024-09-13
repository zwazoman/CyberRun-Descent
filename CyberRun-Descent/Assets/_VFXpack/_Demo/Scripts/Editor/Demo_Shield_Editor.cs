using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEditor;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;


namespace SimpleVFXs
{

#if UNITY_EDITOR

    [CustomEditor(typeof(Demo_Shield))]
    class Demo_Shield_Editor : Editor
    {
        public override void OnInspectorGUI()
        {

            CustomEditorHelper.drawInfoPanel("This script is meant to demonstrate how you can use the shield VFX and its \'shieldVisual\' associated script.");

            DrawDefaultInspector();

            Demo_Shield myScript = (Demo_Shield)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Enable the shield"))
            {
                myScript.EnableShield();
            }

            GUILayout.Space(4);
            if (GUILayout.Button("Disable the shield"))
            {
                myScript.DisableShield();
            }

            GUILayout.Space(4);
            if (GUILayout.Button("Deal 20 damage"))
            {
                myScript.Take20Damage();
            }

            GUILayout.Space(4);
            if (GUILayout.Button("Break the shield"))
            {
                myScript.BreakShield();
            }

        }
    }

#endif


}