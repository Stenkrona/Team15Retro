using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class HannasButton : EditorWindow
{
    float uselessFloat;
    int evenMoreUselessInt;
    bool uselessBool;


    [MenuItem("HannasWindow/UselessWindow")]
    public static void ShowWindow()
    {
        GetWindow<HannasButton>();
    }

    void OnGUI()
    {
        GUILayout.Label("Useless Settings", EditorStyles.boldLabel);
        uselessFloat = EditorGUILayout.Slider("Useless Float", uselessFloat, 0, 100);
        evenMoreUselessInt = EditorGUILayout.IntSlider("Even More Useless Int", evenMoreUselessInt, 0, 30);
        uselessBool = GUILayout.Button("Useless button");

        if (GUILayout.Button("Want Useless Popups?"))
            EditorUtility.DisplayDialog("Here you go!", "This is really a useless popup!", "Click here", "Or here");
    }
}
#endif