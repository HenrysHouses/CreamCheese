/*
 * Edited by:
 * Henrik
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Card_Selector : MonoBehaviour
{
    private Card_Behaviour CB;
    private Animator anim;
    [SerializeField] EventReference cardflip;
    public ParamRef pp;
    public Vector3 targetHandPos;
    public Vector3 targetHandLocalPos;
    private BoxCollider _collider;

    private CameraMovement cammove;
    private Player_Hand playerhand;

    public bool holdingOver;


    private void Start()
    {
        CB = gameObject.GetComponentInChildren<Card_Behaviour>();
        anim = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();
        cammove = Camera.main.GetComponent<CameraMovement>();
        playerhand = GetComponentInParent<Player_Hand>();

    }

    private void Update()
    {


        if (cammove.cameraViewContainer != CameraView.Down)
        {
            anim.SetBool("CardForward", false);

        }


    }

    public void setHandPos(Vector3 localPos)
    {
        transform.parent.localPosition = localPos;
        targetHandPos = transform.parent.position;
        targetHandLocalPos = localPos;
    }


    public void OnMouseEnter()
    {
        if (cammove.cameraViewContainer == CameraView.Down)
        {
            anim.SetBool("CardForward", true);
        }
        else
        {
            if (!this.enabled)
                return;

            holdingOver = true;

            SoundPlayer.PlaySound(cardflip, gameObject);
            // anim.SetBool("SelectedCard", true);
            // Debug.Log("Called");
            _collider.size = new Vector3(0.129058808f, 0.849429905f, 0.012907018f);

        }

    }

    public void OnMouseExit()
    {

        if (cammove.cameraViewContainer == CameraView.Down)
        {
            anim.SetBool("CardForward", false);
            Debug.Log("Touched thohihihi");
            //playerhand.UpdateCards();

        }
        else
        {
            if (!this.enabled)
                return;

            holdingOver = false;
            //SoundManager.Instance.StopSound(cardflip,gameObject);
            //anim.SetBool("SelectedCard", false);
            _collider.size = new Vector3(0.122779235f, 0.569999993f, 0.012907018f);
           // playerhand.UpdateCards();



        }




    }


    public void disableHover()
    {
        anim.enabled = false;
        this.enabled = false;
    }

    public IEnumerator enableHover(Player_Hand hand)
    {
        anim.enabled = true;
        bool isAnimating = true;
        float t = 0;
        holdingOver = false;


        while (isAnimating)
        {
            t += Time.deltaTime;
            if (t >= anim.GetCurrentAnimatorStateInfo(0).length)
            {
                isAnimating = false;
            }
            yield return new WaitForEndOfFrame();
        }
        this.enabled = true;
        hand.UpdateCards();
    }


}