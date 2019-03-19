using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerInspector : Editor
{

    public override void OnInspectorGUI()
    {
        SerializedObject so = serializedObject;

        GUI.color = Color.red;
        if(GUILayout.Button("Don't Press This!"))
        {
            GUI.color = Color.white;
            if (EditorUtility.DisplayDialog("I told you not to press it!", "You did this to yourself! What do you have to say for yourself?", "I'm sorry :(", "I do what I want! :@"))
            {
                AssetDatabase.SaveAssets();
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }
            EditorApplication.Exit(0);
        }
        GUI.color = Color.white;
        base.OnInspectorGUI();
    }

}
