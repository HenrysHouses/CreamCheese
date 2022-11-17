using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHighlight : MonoBehaviour
{
    [SerializeField] Renderer Highlight;
    bool shouldTurnOff = true;

    // Start is called before the first frame update
    void Start()
    {
        Highlight = GetComponent<Renderer>();
        Highlight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        if(shouldTurnOff)
            Highlight.enabled = false;
        else
            shouldTurnOff = true;
    }

    public void EnableHighlight()
    {
        if(this.enabled)
        {
            shouldTurnOff = false;
            Highlight.enabled = true;
        }
    }
}
