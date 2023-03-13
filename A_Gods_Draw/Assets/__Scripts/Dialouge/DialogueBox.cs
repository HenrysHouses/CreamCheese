/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using UnityEngine;
using FMODUnity;
using TMPro;

/// <summary>MonoBehaviour that controls the dialogue box</summary>
public class DialogueBox : MonoBehaviour
{
    public TextMeshPro TextMesh;
    public float TimeBetweenPages = 10;

    [TextArea(5,20), Tooltip("Each dimension is one page of the dialogue")]
    public sentence[] DialogueText;
    private char[] currDialogue;
    int pageIndex = -1;

    // state
    bool isDisplayingPage;
    public bool fullPageIsDisplaying => !isDisplayingPage;
    bool isWaitingForNextPage;
    bool nextPageIsReady = true;
    bool isDialogueInProgress = true;
    EventReference character_SFX;

    Coroutine displayPageRoutine;

    void Update()
    {
        if(!isDialogueInProgress)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            skipPage(pageIndex);
        }

        if(pageIndex < DialogueText.Length)
        {
            if(!isDisplayingPage && nextPageIsReady && pageIndex < DialogueText.Length)
            {
                pageIndex++;
                goToDialoguePage(pageIndex);
                displayPageRoutine = StartCoroutine(DisplayPage());
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
    
        // ! This is not ideal but its ok, no worries :)
        if(DialogueText[pageIndex] is TutorialSentence _sentence)
        {
            yield return new WaitUntil(()=>_sentence.IsComplete);
        }


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

    public void skipPage(int page)
    {   
        if(pageIndex != page)
            return;

        StopCoroutine(displayPageRoutine);
        TextMesh.text = currDialogue.ArrayToString();
        isDisplayingPage =  false;
    }

    void displayChar(int i)
    {
        TextMesh.text += currDialogue[i];
        if(character_SFX.Path != "")
            SoundPlayer.PlaySound(character_SFX, gameObject);
       // Debug.Log("Remove comment here to add sound");
    }

    void goToDialoguePage(int i)
    {
        if(i < DialogueText.Length)
            currDialogue = DialogueText[i].text.ToCharArray();
        else
            currDialogue = new char[0];
        TextMesh.text = "";
    }

    public void SetDialogue(IDialogue dialogue)
    {
        DialogueText = dialogue.pages;
        character_SFX = dialogue.SFX;
    }

    public sentence getCurrentPage()
    {
        if(DialogueText.Length <= pageIndex)
            return null;

        if(pageIndex > -1)
            return DialogueText[pageIndex];
        else
            return DialogueText[0];
    }

    public int getCurrentPageIndex()
    {
        return pageIndex;
    }
}

