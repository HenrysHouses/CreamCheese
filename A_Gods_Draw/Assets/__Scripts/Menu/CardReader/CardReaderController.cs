/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;

public class CardReaderController : MonoBehaviour
{
    Camera MainCam;
    [SerializeField] KeyCode Button = KeyCode.Mouse0;
    [SerializeField] LayerMask layer;
    [SerializeField] PathController path;
    [SerializeField] float cameraOffset = 1;
    [SerializeField] float speed = 1;
    [SerializeField] Vector3 RotationOffset;
    public bool isInspecting => inspectorTarget;
    float inspectorTime;
    Transform inspectorTarget;
    DisableHighlight[] HighlightDisabler;
    public bool keepHighlightsOn = true;
    bool shouldReturn;
    Quaternion cardBaseRot;
    public bool CanSelect = true;

    void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        setPathToCamera();
        FindCard();
        UpdateInspectTarget();
    }

    void setPathToCamera()
    {
        path.endPoint.position = MainCam.transform.position + MainCam.transform.forward * cameraOffset;
        path.recalculatePath();
    }

    void FindCard()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

        if(!Input.GetKeyDown(Button) && CanSelect)
            return;

        if(inspectorTarget != null)
        {
            shouldReturn = true;
            Debug.Log("deselect");
            return;
        }

        if(!Physics.Raycast(ray, out RaycastHit hit, 100, layer))
            return;
        
        ReaderTarget found = hit.collider.GetComponentInChildren<ReaderTarget>();
        Debug.Log(hit.collider.name);

        if(found == null)
            return;

        cardBaseRot = found.transform.rotation;
        inspectorTarget = found.transform;
        // inspectorTarget.transform.rotation = MainCam.transform.rotation;
        inspectorTarget.transform.Rotate(RotationOffset, Space.Self);
        
        HighlightDisabler = inspectorTarget.GetComponents<DisableHighlight>();
        if(HighlightDisabler.Length > 0)
        {
            foreach (var highlight in HighlightDisabler)
            {
                highlight.StayEnabled();
            }
        }

        path.startPoint.position = found.transform.position;
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
        inspectorTarget.position = OP.pos;

        if(shouldReturn && inspectorTime == 0)
        {
            inspectorTarget.transform.localPosition = Vector3.zero;
            inspectorTarget.transform.rotation = cardBaseRot;
            inspectorTarget = null;
        }
    }

    public void returnInspection()
    {
        shouldReturn = true;
    }
}