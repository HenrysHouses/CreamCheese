// *
// * Written By Henrik
// *
// *

using System.Collections.Generic;
using UnityEngine;

public class RuneStoneController : MonoBehaviour
{
    [SerializeField] PlayerTracker playerState;
    [SerializeField] Explosion_VFX GainRuneVFX;
    public List<RuneData> runes;
    public GameObject[] renderers;

    void Awake()
    {
        renderers[0].GetComponent<UIPopup>().setDescription(new StrengthRune(1, RuneState.Active).RuneData.Description);
        renderers[1].GetComponent<UIPopup>().setDescription(new WealthRune(1, RuneState.Active).RuneData.Description);
        renderers[2].GetComponent<UIPopup>().setDescription(new ChaosRune(1, RuneState.Active).RuneData.Description);
    
        playerState.OnPlayerGainRune.AddListener(updateRunes);
    }

    void Start()
    {
        updateRunes(playerState.CurrentRunes, true);
        GainRuneVFX.gameObject.SetActive(false);
    }

    private void getRunes(List<rune> Runes = null)
    {
        if(Runes is null)
            return;

        for (int i = 0; i < Runes.Count; i++)
        {
            if(!runes.Contains(Runes[i].RuneData))
                runes.Add(Runes[i].RuneData);
        }
    }

    public void ToggleRune(RuneType TargetRune)
    {
        for (int i = 0; i < runes.Count; i++)
        {
            if(runes[i].Name != TargetRune)
                continue;
            
            if(runes[i].State.Equals(RuneState.Active))
                runes[i].State = RuneState.Disabled;
            
            if(runes[i].State.Equals(RuneState.Disabled))
                runes[i].State = RuneState.Active;
            break;
        }
    }

    public void SetRune(RuneType TargetRune, RuneState state)
    {
        for (int i = 0; i < runes.Count; i++)
        {
            if(runes[i].Name != TargetRune)
                continue;
            
            runes[i].State = state;
            break;
        }
    }

    void updateRunes(List<rune> currentRunes, bool skipVFX)
    {
        if(runes.Equals(currentRunes))
            return;

        getRunes(currentRunes);

        foreach (var rune in runes)
        {
            foreach (var playRune in currentRunes)
            {
                if(!rune.Name.Equals(playRune.RuneData.Name))
                    continue;

                switch(playRune.RuneData.State)
                {
                    case RuneState.Active:
                        renderers[rune.ID].SetActive(true);
                        if(!skipVFX)
                            playVFX(renderers[rune.ID].transform);
                        break;
                    case RuneState.Temporary:
                        // renderers[rune.ID].setColor(Color.red);
                        break;
                    case RuneState.Disabled:
                        renderers[rune.ID].SetActive(false);
                        break;
                }
            }
        }
    }

    void playVFX(Transform position)
    {
        Debug.Log("playing vfx");
        GainRuneVFX.transform.localPosition = position.localPosition + Vector3.right * 0.2f;
        StartCoroutine(GainRuneVFX.StartExplosion());
    }

    public bool shouldWaitForVFX() => GainRuneVFX.isPlaying;
}

// [System.Serializable]
// public class runeRenderer
// {
//     public Renderer[] renderers;

//     public void setColor(Color color)
//     {
//         foreach (var rend in renderers)
//         {
//             rend.material.color = color;
//         }
//     }
// }