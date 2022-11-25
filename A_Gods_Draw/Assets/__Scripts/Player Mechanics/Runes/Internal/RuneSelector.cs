using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RuneSelector : MonoBehaviour
{
    public TextMeshPro text;
    public rune Rune;
    public void set(RuneType type)
    {
        switch(type)
        {
            case RuneType.FeWealth:
                Rune = new WealthRune(1, RuneState.Active);
                break;            

            case RuneType.TursChaos:
                Rune = new ChaosRune(1, RuneState.Active);
                break;

            // case RuneType.UrrStrength:
            //     Rune = new StrengthRune(1, RuneState.Active);
            //     break;

            default:
                Debug.Log("this rune has not been implemented: " + type);
                break;
        }   
        if(Rune != null)
            text.text = Rune.RuneData.Name.ToString();
        else
            Destroy(gameObject);
    }
}
