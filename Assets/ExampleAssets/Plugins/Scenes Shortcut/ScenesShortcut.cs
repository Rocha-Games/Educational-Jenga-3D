using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class ScenesShortcut : EditorWindow
{
    public ScenesShortcutSO scenesList;
    private Vector2 scrollPos;

    
    [MenuItem("Window/Rocha Games/Scenes Shortcut")]
    private static void Init()
    {
        var unityTab = (ScenesShortcut)GetWindow(typeof(ScenesShortcut));
        unityTab.name = "Scenes Shortcut";
        unityTab.Show();
    }
    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width * 0.15f);
        GUILayout.BeginVertical();

        foreach (var scene in scenesList.sceneLoaderList.Where(scene => GUILayout.Button(scene.sceneName, GUILayout.Height(22))))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(scene.scene));
        }

        GUILayout.EndVertical();
        GUILayout.Space(Screen.width * 0.15f);
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }
}
#endif