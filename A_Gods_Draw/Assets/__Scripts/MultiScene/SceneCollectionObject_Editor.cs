using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SceneCollectionObject))]
public class SceneCollectionObject_Editor : Editor
{
    SceneCollectionObject script;

    private void OnEnable()
    {
        script = target as SceneCollectionObject;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    
        // Drawing custom list
        EditorGUILayout.LabelField("Scenes");
        var _Selected = script.Scenes; // selected scenes
        var _choices = script.ChoiceIndex; // index of selected scenes
        int newCount = Mathf.Max(0, EditorGUILayout.DelayedIntField("    size", _Selected.Count));
        while (newCount < _Selected.Count)
        {
            _Selected.RemoveAt( _Selected.Count - 1 );
            _choices.RemoveAt( _choices.Count - 1 );
        }
        while (newCount > _Selected.Count)
        {
            _Selected.Add(null);
            _choices.Add(0);
        }
 
        for(int i = 0; i < _Selected.Count; i++)
        {
            string[] options = script.getSceneOptions();

            // Drawing dropdown of strings
            _choices[i] = EditorGUILayout.Popup(_choices[i], options);
            // Update the selected choice in the underlying object
            _Selected[i] = options[_choices[i]];
        }

        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }

    void OnInspectorUpdate()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;     
        Debug.Log(sceneCount);
        string[] scenes = new string[sceneCount];

        for( int i = 0; i < sceneCount; i++ )
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension( SceneUtility.GetScenePathByBuildIndex( i ) );
        }
        script.setSceneOptions(scenes);
    }
}
