using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntw.CurvedTextMeshPro;

public class KeepingTheTextAligned : MonoBehaviour
{
     private TextProOnAExp expBase;
     private Card_Selector card;

    // Start is called before the first frame update
    void Start()
    {
        expBase = GetComponent<TextProOnAExp>();
        card = GetComponentInParent<Card_Selector>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(expBase.m_expBase != 1.16)
        {
            expBase.m_expBase = 1.16f;

            if(expBase.m_expBase == 1.16f)
            {
                expBase.m_expBase = 1.17f;
            }
           // Debug.Log("Why wont the text align??");
        }
    }
}
