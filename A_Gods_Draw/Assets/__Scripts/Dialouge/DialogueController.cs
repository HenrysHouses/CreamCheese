using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HH.MultiSceneTools;

using System;

public class DialogueController : MonoBehaviour
{
    event Action<GameObject> event_;

    public static DialogueController instance; 
    [SerializeField] GameObject dialoguePrefab;

    [SerializeField] DialogueTransform[] dialogueTransform;

    void Start()
    {
        UpdateTransforms();
        MultiSceneLoader.OnSceneCollectionLoaded.AddListener(UpdateTransforms);

        if(instance == null)
            instance = this;
        else
        {
            Debug.LogWarning(gameObject.name + " Was destroyed: Two instances of " + this.name);
            Destroy(gameObject);
        }
    }

    void UpdateTransforms()
    {
        dialogueTransform = GameObject.FindObjectsOfType<DialogueTransform>();
    }

    public void SpawnDialogue(Dialogue dialogue)
    {
        foreach (var holder in dialogueTransform)
        {
            if(holder.TransformName == dialogue.TransformName)
            {
                if(holder.hasDialogue())
                    return;

                GameObject spawn = Instantiate(dialoguePrefab);
                spawn.GetComponent<DialogueBox>().SetDialogue(dialogue);
                spawn.transform.SetParent(holder.transform);
                spawn.transform.localPosition = Vector3.zero;
                spawn.transform.localRotation = Quaternion.identity;
                return;
            }
        }
        Debug.LogWarning(dialogue.TransformName + " Dialogue Transform was not found");
    }

    void OnDestroy()
    {
        // MultiSceneLoader.OnSceneLoad.RemoveListener(UpdateTransforms);
    }
}
