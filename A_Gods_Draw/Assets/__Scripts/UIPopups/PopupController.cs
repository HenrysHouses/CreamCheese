using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] RectTransform _transform;
    public PopupLocation PreferredPosition;
    Vector2 PivotOpposite => new Vector2(_transform.rect.width, _transform.rect.height);
    Vector2 Pivot => new Vector2(0, 0);
    Vector2 PivotAbove => new Vector2(0, _transform.rect.height);
    Vector2 PivotSide => new Vector2(_transform.rect.width, 0);
    
    void Update()
    {
        bool leftTopCorner;
        bool leftBottomCorner;
        bool rightTopCorner;
        bool rightBottomCorner;

        bool outOfBounds = checkOutOfBounds(out leftTopCorner, out leftBottomCorner, out rightTopCorner, out rightBottomCorner);

        if(!outOfBounds)
            return;

        int locations = PopupLocation.GetNames(typeof(PopupLocation)).Length;

        for (int i = 0; i < locations; i++)
        {
            SetAnchor((PopupLocation)i);
            outOfBounds = checkOutOfBounds(out leftTopCorner, out leftBottomCorner, out rightTopCorner, out rightBottomCorner);
            if(!outOfBounds)
                return;
        }
    }

    /// <summary>Checks if any corner of the UI is out of bounds of the screen</summary>
    /// <returns>true if any corner is out of bounds. out LeftTop, LeftBottom, RightTop, RightBottom shows which corner is out of bounds</returns>
    bool checkOutOfBounds(out bool LeftTop, out bool LeftBottom, out bool RightTop, out bool RightBottom)
    {
        // World positions of the corners of the popup
        var _LeftBottomPos = Camera.main.WorldToScreenPoint(GetLeftBottomCornerPosition());
        var _LeftTopPos = Camera.main.WorldToScreenPoint(GetLeftTopCornerPosition());
        var _RightBottomPos = Camera.main.WorldToScreenPoint(GetRightBottomCornerPosition());
        var _RightTopPos = Camera.main.WorldToScreenPoint(GetRightTopCornerPosition());

        LeftBottom = !Screen.safeArea.Contains(_LeftBottomPos);
        LeftTop = !Screen.safeArea.Contains(_LeftTopPos);
        RightBottom = !Screen.safeArea.Contains(_RightBottomPos);
        RightTop = !Screen.safeArea.Contains(_RightTopPos);

        // Checks if any of the corners are out
        return LeftBottom || LeftTop || RightBottom || RightTop;
    }

    /// <summary>Changes the rect transform anchor and pivot so the popup appears in the correct position</summary>
    /// <param name="anchor">Position in relation to the cursor</param>
    public void SetAnchor(PopupLocation anchor)
    {
        switch(anchor)
        {
            case PopupLocation.LeftBottom:
                _transform.anchorMin = new Vector2(1,1);
                _transform.anchorMax = new Vector2(1,1);
                _transform.pivot = new Vector2(1,1);
                transform.localPosition = new Vector3(-950,-950,0);
                break;
            case PopupLocation.LeftTop:
                _transform.anchorMin = new Vector2(1,0);
                _transform.anchorMax = new Vector2(1,0);
                _transform.pivot = new Vector2(1,0);
                transform.localPosition = new Vector3(-950,50,0);
                break;
            case PopupLocation.RightBottom:
                _transform.anchorMin = new Vector2(0,1);
                _transform.anchorMax = new Vector2(0,1);
                _transform.pivot = new Vector2(0,1);
                transform.localPosition = new Vector3(50,-950,0);
                break;
            case PopupLocation.RightTop:
                _transform.anchorMin = new Vector2(0,0);
                _transform.anchorMax = new Vector2(0,0);
                _transform.pivot = new Vector2(0,0);
                transform.localPosition = new Vector3(50,50,0);
                break;
        }

        PreferredPosition = anchor;
    }

#region World Position Getters

    // Gets the correct world position depending on what the anchor and pivot is set to.
    public Vector3 GetLeftBottomCornerPosition() 
    {
        Vector2 pos = Vector2.one * -1;

        switch(PreferredPosition)
        {
            case PopupLocation.RightTop:
                pos = Pivot;
                break;
            
            case PopupLocation.LeftBottom:
                pos = -PivotOpposite;
                break;
            
            case PopupLocation.LeftTop:
                pos = -PivotSide;
                break;
            
            case PopupLocation.RightBottom:
                pos = -PivotAbove;
                break;
        }

        Vector3 LocalPos = pos;
        LocalPos.z = 0;
        return _transform.TransformPoint(LocalPos);
    }

    // Gets the correct world position depending on what the anchor and pivot is set to.
    public Vector3 GetLeftTopCornerPosition() 
    {
        Vector2 pos = Vector2.one * -1;

        switch(PreferredPosition)
        {
            case PopupLocation.RightTop:
                pos = PivotAbove;
                break;

            case PopupLocation.LeftBottom:
                pos = -PivotSide;
                break;

            case PopupLocation.LeftTop:
                pos =  new Vector2(-PivotOpposite.x, PivotOpposite.y);
                break;

            case PopupLocation.RightBottom:
                pos = Pivot;
                break;
        }
        
        Vector3 LocalPos = pos;
        LocalPos.z = 0;
        return _transform.TransformPoint(LocalPos);
    }

    // Gets the correct world position depending on what the anchor and pivot is set to.
    public Vector3 GetRightBottomCornerPosition() 
    {
        Vector2 pos = Vector2.one * -1;

        switch(PreferredPosition)
        {
            case PopupLocation.RightTop:
                pos = PivotSide;
                break;

            case PopupLocation.LeftBottom:
                pos = -PivotAbove;
                break;

            case PopupLocation.LeftTop:
                pos = Pivot;
                break;

            case PopupLocation.RightBottom:
                pos = new Vector2(PivotOpposite.x, -PivotOpposite.y);
                break;
        }

        Vector3 LocalPos = pos;
        LocalPos.z = 0;
        return _transform.TransformPoint(LocalPos);
    }

    // Gets the correct world position depending on what the anchor and pivot is set to.
    public Vector3 GetRightTopCornerPosition() 
    {
        Vector2 pos = Vector2.one * -1;

        switch(PreferredPosition)
        {
            case PopupLocation.RightTop:
                pos = PivotOpposite;
                break;

            case PopupLocation.LeftBottom:
                pos = Pivot;
                break;

            case PopupLocation.LeftTop:
                pos = PivotAbove;
                break;

            case PopupLocation.RightBottom:
                pos = PivotSide;
                break;
        }

        Vector3 LocalPos = pos;
        LocalPos.z = 0;
        return _transform.TransformPoint(LocalPos);
    }
#endregion
}
