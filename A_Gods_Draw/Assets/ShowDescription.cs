using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowDescription : MonoBehaviour
{
    Canvas description;
    TMP_Text descText;

    private void Start()
    {
        description = transform.parent.parent.GetComponentInChildren<Canvas>();
        descText = description.GetComponentInChildren<TMP_Text>();
    }

    public void SetText(string newDesc)
    {
        descText.text = newDesc;
    }

    private void OnMouseEnter()
    {
        description.enabled = true;
    }

    private void OnMouseExit()
    {
        description.enabled = false;
    }
}
