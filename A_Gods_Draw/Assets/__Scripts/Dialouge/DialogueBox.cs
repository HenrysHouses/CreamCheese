using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public TextMeshPro TextMesh;
    public float TimeBetweenPages = 1;

    [TextArea(5,20), Tooltip("Each dimension is one page of the dialogue")]
    public sentence[] DialogueText;
    private char[] currDialogue;
    int pageIndex = -1;

    // state
    bool isDisplayingPage;
    bool isWaitingForNextPage;
    bool nextPageIsReady = true;
    bool isDialogueInProgress = true;
    EventReference character_SFX;

    void Update()
    {
        if(!isDialogueInProgress)
            return;

        if(pageIndex < DialogueText.Length)
        {
            if(!isDisplayingPage && nextPageIsReady && pageIndex < DialogueText.Length)
            {
                pageIndex++;
                goToDialoguePage(pageIndex);
                StartCoroutine(DisplayPage());
                return;
            }
            
            if(!isDisplayingPage && !nextPageIsReady && !isWaitingForNextPage)
            {
                StartCoroutine(WaitForNextPage());
                return;
            }
        }
        else if (isDialogueInProgress)
        {
            isDialogueInProgress = false;
            Destroy(gameObject, TimeBetweenPages);
        }
    }

    IEnumerator WaitForNextPage()
    {
        isWaitingForNextPage = true;
        yield return new WaitForSeconds(TimeBetweenPages);
        nextPageIsReady = true;
        isWaitingForNextPage = false;
    }

    IEnumerator DisplayPage()
    {
        nextPageIsReady = false;
        isDisplayingPage = true;
        int _char = 0;

        while(_char < currDialogue.Length)
        {
            displayChar(_char);
            yield return new WaitForSeconds(DialogueText[pageIndex].speed);
            _char++;
        }
        isDisplayingPage = false;
    }

    void displayChar(int i)
    {
        TextMesh.text += currDialogue[i];
        //SoundPlayer.PlaySound(character_SFX, gameObject);
        Debug.Log("Remove comment here to add sound");
    }

    void goToDialoguePage(int i)
    {
        if(i < DialogueText.Length)
            currDialogue = DialogueText[i].text.ToCharArray();
        else
            currDialogue = new char[0];
        TextMesh.text = "";
    }

    public void SetDialogue(Dialogue dialogue)
    {
        DialogueText = dialogue.pages;
        character_SFX = dialogue.SFX;
    }
}

