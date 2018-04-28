using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoadingScreen))]
public class LoadingScreenEditor : Editor {

    bool showHelp;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        EditorGUILayout.LabelField("Loading Screen", EditorStyles.helpBox);

        GUIStyle bSKin = new GUIStyle("box");
        bSKin.normal.textColor = Color.green;

        if (GUILayout.Button("[?]", bSKin))
        {
            showHelp = !showHelp;
        }

        EditorGUILayout.EndHorizontal();

        if (showHelp)
        {
            EditorGUILayout.HelpBox("This script handles all the loading / unloading tasks.\nIf you have to go to " +
                "a scene or go back to main menu from your scene " +
                "or you have to move in between different scenes " +
                "just simply save the SceneName you want to load :\n\n" +
                "PlayerPrefs.SetString(sceneToLoad, yourSceneName);\n\n" +
                "and then load the loading scene like this :\n\n" +
                "Fader fader = FindObjectOfType<Fader>(); \n" +
                "fader.FadeIntoLevel(LoadingScreen); \n\n" +
                "That's it! \n" +
                "For more info please see the 'newGame()' method of " +
                "MainMenuController script!", MessageType.Info);
        }

        base.OnInspectorGUI();

        EditorGUILayout.EndVertical();
    }

}
