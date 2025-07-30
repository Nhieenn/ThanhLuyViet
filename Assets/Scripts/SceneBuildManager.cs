using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneBuildManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string[] sceneNames = { "Main Menu", "LV1", "LV2", "LV3", "LV4", "LV5", "LV6", "LV7", "LV8", "Lv9", "Lv10", "FirebaseLogin" };
    
    [Header("Debug")]
    public bool checkScenesOnStart = true;
    
    void Start()
    {
        if (checkScenesOnStart)
        {
            CheckAndLogScenes();
        }
    }
    
    void CheckAndLogScenes()
    {
        Debug.Log("=== Scene Build Settings Check ===");
        
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            Debug.Log($"Build Index {i}: {sceneName}");
        }
        
        Debug.Log("=== Available Scenes ===");
        foreach (string sceneName in sceneNames)
        {
            bool found = false;
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string buildSceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (buildSceneName == sceneName)
                {
                    Debug.Log($"✓ {sceneName} (Index: {i})");
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Debug.LogWarning($"✗ {sceneName} - NOT IN BUILD SETTINGS!");
            }
        }
    }
    
    public void LoadScene(string sceneName)
    {
        // Kiểm tra scene có trong build settings không
        bool sceneExists = false;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string buildSceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (buildSceneName == sceneName)
            {
                sceneExists = true;
                Debug.Log($"Loading scene: {sceneName} (Index: {i})");
                SceneManager.LoadScene(sceneName);
                break;
            }
        }
        
        if (!sceneExists)
        {
            Debug.LogError($"Scene '{sceneName}' is not in Build Settings!");
            Debug.LogError("Please add the scene to Build Settings: File -> Build Settings");
        }
    }
    
    public void LoadSceneByIndex(int buildIndex)
    {
        if (buildIndex >= 0 && buildIndex < SceneManager.sceneCountInBuildSettings)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            Debug.Log($"Loading scene by index: {sceneName} (Index: {buildIndex})");
            SceneManager.LoadScene(buildIndex);
        }
        else
        {
            Debug.LogError($"Invalid build index: {buildIndex}");
        }
    }
    
    // Hàm để test load scene an toàn
    public void TestLoadScene(string sceneName)
    {
        Debug.Log($"Attempting to load scene: {sceneName}");
        LoadScene(sceneName);
    }
} 