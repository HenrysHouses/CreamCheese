using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSpawnTest : MonoBehaviour
{
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        DialogueController.instance.SpawnDialogue(dialogue);
    }
}
