using UnityEngine;

public class UIPopup : MonoBehaviour
{
    public Popup_ScriptableObject PopupInfo;
    public bool CreateInstance, instanceCreated;
    bool hasPopup;
    
    void Start()
    {
        BoardElementsInfo info = GetComponent<BoardElementsInfo>();
        
        if(info)
        {
            if(CreateInstance)
            {
                info.PopupInfo.Clone(ref PopupInfo);
            }
            else
                PopupInfo = info.PopupInfo;
        }
        else if(PopupInfo != null && CreateInstance)
        {
            Popup_ScriptableObject temp = ScriptableObject.CreateInstance<Popup_ScriptableObject>();
            PopupInfo.Clone(ref temp);
            temp.name = temp.name + " (instance)";
            PopupInfo = temp;
        }
    }

    void OnMouseEnter()
    {
        if(PopupInfo)
        {
            PopupHold.instance.StartPopup(PopupInfo);
            hasPopup = true;
        }
    }

    void OnMouseExit()
    {
        if(PopupInfo && hasPopup)
            PopupHold.instance.StopPopup();
    }

    void OnDestroy()
    {
        if(PopupInfo && hasPopup)
            PopupHold.instance.StopPopup();
    }

    void OnDisable()
    {
        if(PopupInfo && hasPopup)
            PopupHold.instance.StopPopup();
    }

    public void setDescription(string description)
    {
        if(!instanceCreated)
        {
            Popup_ScriptableObject tempInfo = PopupInfo;
            PopupInfo = ScriptableObject.CreateInstance<Popup_ScriptableObject>();
            tempInfo.Clone(ref PopupInfo);
            instanceCreated = true;
        }

        PopupInfo.Info = description;
    }
}
