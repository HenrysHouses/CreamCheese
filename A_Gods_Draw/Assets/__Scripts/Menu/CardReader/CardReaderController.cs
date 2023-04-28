/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;

public class CardReaderController : MonoBehaviour
{
    Camera MainCam;
    [SerializeField] KeyCode Button = KeyCode.Mouse0, CancelButton = KeyCode.Mouse1;
    [SerializeField] LayerMask layer;
    [SerializeField] PathController path;
    [SerializeField] float cameraOffset = 1, cameraXOffset = 0, cameraYOffset = 0;
    [SerializeField] float speed = 1;
    [SerializeField] bool RotateUpRight;
    [SerializeField] Vector3 RotationOffset;
    public bool isInspecting => inspectorTarget;
    float inspectorTime;
    ReaderTarget inspectorTarget;
    public ReaderTarget getTarget() => inspectorTarget;
    DisableHighlight[] HighlightDisabler;
    public bool keepHighlightsOn = true;
    public bool returnTargetToOrigin = true;
    bool shouldReturn;
    public bool CanSelect = true;

    void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.PauseMenuIsOpen)
        {
            return;
        }

        setPathToCamera();
        FindCard();
        UpdateInspectTarget();
    }

    void setPathToCamera()
    {
        path.endPoint.position = MainCam.transform.position + (MainCam.transform.forward * cameraOffset) + (MainCam.transform.right * cameraXOffset) + (MainCam.transform.up * cameraYOffset);
        path.recalculatePath();
    }

    void FindCard()
    {
        if(inspectorTarget)
            if(Input.GetKeyDown(CancelButton) && CanSelect)
                returnInspection();

        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

        if(!Input.GetKeyDown(Button) && CanSelect)
            return;

        if(inspectorTarget != null)
        {
            shouldReturn = true;
            return;
        }

        if(!Physics.Raycast(ray, out RaycastHit hit, 100, layer))
            return;
        
        ReaderTarget _target = hit.collider.GetComponentInChildren<ReaderTarget>();

        if(_target == null)
            return;

        // inspectorTarget.transform.rotation = MainCam.transform.rotation;
        _target.transform.Rotate(RotationOffset, Space.Self);
        
        HighlightDisabler = _target.GetComponents<DisableHighlight>();
        if(HighlightDisabler.Length > 0)
        {
            foreach (var highlight in HighlightDisabler)
            {
                highlight.StayEnabled();
            }
        }

        path.startPoint.position = _target.transform.position;
        inspectorTarget = _target;
        inspectorTarget.isBeingInspected = true;
        path.recalculatePath();
        shouldReturn = false;
    }

    void UpdateInspectTarget()
    {
        if(!inspectorTarget)
            return;

        if(shouldReturn)
            inspectorTime = Mathf.Clamp01(inspectorTime - Time.deltaTime * speed);
        else
            inspectorTime = Mathf.Clamp01(inspectorTime + Time.deltaTime * speed);
    
        if(HighlightDisabler.Length > 0 && keepHighlightsOn)
        {
            foreach (var highlight in HighlightDisabler)
            {
                if(!highlight)
                    continue;
                highlight.highlight.SetActive(true);
                highlight.StayEnabled();
            }
        }

        OrientedPoint OP = path.GetEvenPathOP(inspectorTime);

        if(RotateUpRight)
            inspectorTarget.transform.localRotation = Quaternion.Lerp(inspectorTarget.DefaultRotation, inspectorTarget.UpRightRotation, inspectorTime);
        inspectorTarget.transform.position = OP.pos;

        if(inspectorTime == 1 && !shouldReturn)
            inspectorTarget.isWaitingToReturn = true;
        else if(shouldReturn)
            inspectorTarget.isWaitingToReturn = false;

        if(shouldReturn && inspectorTime == 0)
        {
            if(returnTargetToOrigin)
                inspectorTarget.transform.localPosition = Vector3.zero;
            inspectorTarget.transform.localRotation = inspectorTarget.DefaultRotation;
            inspectorTarget.isWaitingToReturn = false;
            inspectorTarget.isBeingInspected = false;
            inspectorTarget = null;
        }
    }

    public void returnInspection()
    {
        shouldReturn = true;
    }

    void OnDestroy()
    {
        if(inspectorTarget)
            inspectorTarget.isBeingInspected = false;
    }
}