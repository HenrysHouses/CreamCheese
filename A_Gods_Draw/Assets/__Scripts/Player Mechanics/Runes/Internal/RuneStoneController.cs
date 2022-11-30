using System.Collections.Generic;
using UnityEngine;

public class RuneStoneController : MonoBehaviour
{
    [SerializeField] PlayerTracker playerState;
    public List<RuneData> runes;
    public runeRenderer[] renderers;

    void Update()
    {
        updateRunes(playerState);
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

    void updateRunes(PlayerTracker player)
    {
        getRunes(player.CurrentRunes);

        foreach (var rune in runes)
        {
            foreach (var playRune in player.CurrentRunes)
            {
                if(!rune.Name.Equals(playRune.RuneData.Name))
                    continue;

                switch(playRune.RuneData.State)
                {
                    case RuneState.Active:
                        renderers[rune.ID].setColor(Color.cyan);
                        break;
                    case RuneState.Temporary:
                        renderers[rune.ID].setColor(Color.red);
                        break;
                    case RuneState.Disabled:
                        renderers[rune.ID].setColor(Color.gray);
                        break;
                }
            }
        }        
    }
}

[System.Serializable]
public class runeRenderer
{
    public Renderer[] renderers;

    public void setColor(Color color)
    {
        foreach (var rend in renderers)
        {
            rend.material.color = color;
        }
    }
}