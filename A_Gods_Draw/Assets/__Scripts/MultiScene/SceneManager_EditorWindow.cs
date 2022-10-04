#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class SceneManager_window : EditorWindow
{
    Scene[] LoadedScenes;
    string[] _sceneOptions;
    int SelectedScene;
    SceneAsset[] currLoadedAssets;
    string[] buildSceneOptions;
    SceneCollectionObject[] _Collection;
    string[] Collection = new string[0];
    SceneCollectionObject LoadedCollection;
    SceneCollectionObject SelectedCollection;
    int UnloadScene;

    [MenuItem("Multi Scene Workflow/Scene Manager")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SceneManager_window window = (SceneManager_window)EditorWindow.GetWindow(typeof(SceneManager_window));
        window.titleContent = new GUIContent("Scene Manager", "Loads, Unloads, and Saves Scene Collections");
        window.Show();
    }

    void OnGUI()
    {
        if(LoadedCollection)
            EditorGUILayout.TextField("Current Loaded Collection: ", LoadedCollection.Title, EditorStyles.boldLabel);
        else
            EditorGUILayout.TextField("Current Loaded Collection: ", "None", EditorStyles.boldLabel);

        // Load Collection
        GUILayout.Space(8);
        GUILayout.Label("Loading", EditorStyles.boldLabel);

        SelectedCollection = (SceneCollectionObject)EditorGUILayout.ObjectField(new GUIContent("Collection"), SelectedCollection, typeof(SceneCollectionObject), false);
        
        if(GUILayout.Button("Load Collection"))
        {
            if(SelectedCollection == null)
            {
                EditorSceneManager.OpenScene("Assets/~Scenes/SampleScene.unity", OpenSceneMode.Single);
                LoadedCollection = null;
                return;
            }

            LoadedCollection = SelectedCollection;

            string[] paths = new string[LoadedCollection.Scenes.Count];
            
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = AssetDatabase.GetAssetPath(LoadedCollection.Scenes[i]);
            }

            for (int i = 0; i < paths.Length; i++)
            {
                if(i == 0)
                    EditorSceneManager.OpenScene(paths[i], OpenSceneMode.Single);
                else
                    EditorSceneManager.OpenScene(paths[i], OpenSceneMode.Additive);
            }

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = LoadedCollection;
        }
        
        // Load Scene

        int sceneCount = SceneManager.sceneCountInBuildSettings;     

        if(_sceneOptions == null)
            _sceneOptions = new string[0];

        if(sceneCount > _sceneOptions.Length)
        {
            string[] scenes = new string[sceneCount];

            for( int i = 0; i < sceneCount; i++ )
            {
                scenes[i] = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );
            }
            _sceneOptions = scenes;
        }

        // # Change this into a scene object field

        if(_sceneOptions.Length > 0)
            SelectedScene = EditorGUILayout.Popup(new GUIContent("Scene") , SelectedScene, _sceneOptions);
        else
            EditorGUILayout.Popup(0, new string[]{"Unload Select"});

        if(GUILayout.Button("Load Scene Additively"))
        {
            string path = GetScenePath(_sceneOptions[SelectedScene]);
            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        }

        // Un-Load selected scene
        GUILayout.Space(8);
        GUILayout.Label("Un-Loading", EditorStyles.boldLabel);

        if(buildSceneOptions != null)
        {
            if(buildSceneOptions.Length > 0)
                UnloadScene = EditorGUILayout.Popup(UnloadScene, buildSceneOptions);
        }
        else
            EditorGUILayout.Popup(0, new string[]{"Unload Select"});

        if(GUILayout.Button("Unload Scene"))
        {
            if(EditorSceneManager.sceneCount > 1)
            {
                EditorSceneManager.CloseScene(LoadedScenes[UnloadScene], true);
            }
            else
                EditorSceneManager.OpenScene("Assets/~Scenes/SampleScene.unity", OpenSceneMode.Single);
        }

        // Saving
        GUILayout.Space(8);
        GUILayout.Label("Saving", EditorStyles.boldLabel);

        if(GUILayout.Button("Save Collection"))
        {
            if(LoadedCollection)
            {
                currLoadedAssets = GetSceneAssetsFromPaths(GetLoadedScenePaths());
                LoadedCollection.saveCollection(currLoadedAssets);
            }
        }



        if(GUILayout.Button("Create Collection From Loaded Scenes"))
        {
            ScriptableObject SO = CreateInstance(typeof(SceneCollectionObject));
            SceneCollectionObject _NewCollection = SO as SceneCollectionObject;
            _NewCollection.saveCollection(currLoadedAssets);
            _NewCollection.Title = "Collection Nr " + _Collection.Length;

            string asset = string.Format("Assets/Resources/SceneCollections/SceneCollectionObject ({0}).asset", _Collection.Length);
            AssetDatabase.CreateAsset(_NewCollection, asset);
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = _NewCollection;
        }

        // Scenes
        GUILayout.Space(8);
        GUILayout.Label("Other", EditorStyles.boldLabel);

        if(GUILayout.Button("Add Open Scenes To Build"))
        {
            SetEditorBuildSettingsScenes();
        }

        if(GUILayout.Button("Create New Scene"))
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            EditorSceneManager.SaveOpenScenes();

            SetEditorBuildSettingsScenes();
            // # "Assets/~Scenes/GameScenes/New Scene.asset");
        }
    }
    
    public void SetEditorBuildSettingsScenes()
    {
        // Find valid Scene paths and make a list of EditorBuildSettingsScene
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        int _sceneCount = SceneManager.sceneCountInBuildSettings;     

        for (int i = 0; i < _sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            if (!string.IsNullOrEmpty(scenePath))
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        }

        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            Scene _addScene = EditorSceneManager.GetSceneAt(i);
            bool found = false;

            foreach (EditorBuildSettingsScene item in editorBuildSettingsScenes)
            {
                if(item.path.Equals(_addScene.path))
                {
                    found = true;
                    break;
                }
                
            }
            if(!found)
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(_addScene.path, true));            
        }

        // Set the Build Settings window Scene list
        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
    }


    void OnInspectorUpdate()
    {
        _Collection = Resources.LoadAll<SceneCollectionObject>("SceneCollections");
        // Storing collection Names
        Collection = new string[_Collection.Length + 1];
        Collection[0] = "Empty";
        for (int i = 1; i < _Collection.Length + 1; i++)
        {
            Collection[i] = _Collection[i-1].Title;    
        }
        // Storing loaded scenes
        int sceneCount = EditorSceneManager.sceneCount;     

        LoadedScenes = new Scene[sceneCount+1];
        buildSceneOptions = new string[sceneCount+1];
        buildSceneOptions[0] = "Select";
        for (int i = 0; i < sceneCount; i++)
        {
            LoadedScenes[i+1] = EditorSceneManager.GetSceneAt(i);
            buildSceneOptions[i+1] = LoadedScenes[i+1].name;
        }
    }

    SceneAsset[] GetSceneAssetsFromPaths(string[] ScenePaths)
    {
        int buildSceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        SceneAsset[] _assets = new SceneAsset[ScenePaths.Length];

        for (int i = 0; i < ScenePaths.Length; i++)
        {
            _assets[i] = (SceneAsset)AssetDatabase.LoadAssetAtPath(ScenePaths[i], typeof(SceneAsset));
        }
        return _assets;
    }

    string[] GetLoadedScenePaths()
    {
        Scene[] _currScenes = new Scene[EditorSceneManager.loadedSceneCount];
        for (int i = 0; i < _currScenes.Length; i++)
        {
            _currScenes[i] = EditorSceneManager.GetSceneAt(i);
        }

        string[] _paths = new string[_currScenes.Length];

        for (int i = 0; i < _paths.Length; i++)
        {
            _paths[i] = _currScenes[i].path;
        }
        return _paths;
    }

    string GetScenePath(string scene)
    {
        int buildSceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        for (int i = 0; i < buildSceneCount; i++)
        {
            string currScene = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );
            if(currScene.Equals(scene))
            {
                return UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i );
            }
        }
        return null;
    }
}
#endif