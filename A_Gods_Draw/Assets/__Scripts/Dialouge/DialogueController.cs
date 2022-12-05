/* 
 * Written by 
 * Henrik
*/

using UnityEngine;
using HH.MultiSceneTools;

/// <summary>MonoBehaviour which spawns in requested dialogue at specified locations</summary>
public class DialogueController : MonoBehaviour
{
    public static DialogueController instance; 
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] GameObject dialoguePrefabUI;

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

    public void SpawnDialogue(Dialogue dialogue, Vector2 rectAnchor)
    {
        foreach (var holder in dialogueTransform)
        {
            if(holder.TransformName == dialogue.TransformName)
            {
                if(holder.hasDialogue())
                    return;

                GameObject spawn = Instantiate(dialoguePrefabUI);
                spawn.GetComponent<DialogueBox>().SetDialogue(dialogue);
                spawn.transform.SetParent(holder.transform);

                if(rectAnchor != Vector2.negativeInfinity)
                {
                    RectTransform _transform = spawn.GetComponent<RectTransform>();
                    _transform.anchoredPosition = rectAnchor;
                    _transform.localPosition = Vector3.zero;
                    _transform.localRotation = Quaternion.identity;
                    return;
                }
            }
        }
        Debug.LogWarning(dialogue.TransformName + " Dialogue Transform was not found");
    }

    void OnDestroy()
    {
        // MultiSceneLoader.OnSceneLoad.RemoveListener(UpdateTransforms);
    }
}
