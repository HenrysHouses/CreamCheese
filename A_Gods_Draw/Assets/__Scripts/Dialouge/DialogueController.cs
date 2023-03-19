/* 
 * Written by 
 * Henrik
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using HH.MultiSceneTools;

/// <summary>MonoBehaviour which spawns in requested dialogue at specified locations</summary>
public class DialogueController : MonoBehaviour
{
    public static DialogueController instance; 
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] GameObject dialoguePrefabUI;

    [SerializeField] DialogueTransform[] dialogueTransform;

    void Awake()
    {
        UpdateTransforms(null, collectionLoadMode.Replace);
        MultiSceneLoader.OnSceneCollectionLoaded.AddListener(UpdateTransforms);
        SceneManager.sceneLoaded += UpdateTransforms;

        if(instance == null)
            instance = this;
        else
        {
            Debug.LogWarning(gameObject.name + " Was destroyed: Two instances of " + this.name);
            Destroy(gameObject);
        }
    }
   
    void UpdateTransforms(SceneCollection collection, collectionLoadMode mode)
    {
        Debug.Log("dasjkldashjkadhj");
        dialogueTransform = GameObject.FindObjectsOfType<DialogueTransform>();
    }

    void UpdateTransforms(Scene collection, LoadSceneMode mode)
    {
        dialogueTransform = GameObject.FindObjectsOfType<DialogueTransform>();
    }

    public DialogueBox SpawnDialogue(IDialogue dialogue, bool replace = false)
    {
        foreach (var holder in dialogueTransform)
        {
            if(holder.TransformName == dialogue.TransformName)
            {
                if(holder.hasDialogue())
                {
                    if(replace)
                        holder.destroyDialogue();
                    else
                        return null;
                }

                GameObject spawn = Instantiate(dialoguePrefab);
                DialogueBox box = spawn.GetComponent<DialogueBox>(); 
                box.SetDialogue(dialogue);
                spawn.transform.SetParent(holder.transform);
                spawn.transform.localScale = Vector3.one;
                spawn.transform.localPosition = Vector3.zero;
                spawn.transform.localRotation = Quaternion.identity;
                return box;
            }
        }
        Debug.LogWarning(dialogue.TransformName + " Dialogue Transform was not found");
        return null;
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
                spawn.transform.localScale = Vector3.one;

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
