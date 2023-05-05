/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;
using TMPro;

public class RuneSelector : MonoBehaviour
{
    public TextMeshPro RuneName;
    public UIPopup RuneDescription;
    [HideInInspector] public rune Rune;
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

        Debug.Log(Rune.hasTriggeredThisGame);

        if(Rune.GetType() != typeof(rune))
        {
            RuneName.text = Rune.RuneData.Name.ToString();
            RuneDescription.setDescription(Rune.RuneData.Description);
        }
        else
        {
            Debug.Log("Rune not implemented: destroyed");
            Destroy(gameObject);
        }
    }
}
