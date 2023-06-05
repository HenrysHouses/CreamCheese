using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntw.CurvedTextMeshPro;
using TMPro;

public class KeepingTheTextAligned : MonoBehaviour
{
     private TextProOnAExp expBase;
     private Card_Selector card;

     private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {

        expBase = GetComponent<TextProOnAExp>();
        card = GetComponentInParent<Card_Selector>();
        text = GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        expBase.ParametersHaveChanged();
       // text.ForceMeshUpdate();
        
        
    }


    
}
