using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CreateAssetMenu(menuName = "Scenes Metadata", fileName = "Scenes Metadata")]
public class ScenesShortcutSO : ScriptableObject
{
    [Serializable]
    public class SceneMetadata
    {
        public string sceneName;

#if UNITY_EDITOR
        public SceneAsset scene;
#endif
    }

    public List<SceneMetadata> sceneLoaderList = new List<SceneMetadata>();
    
}
#endif