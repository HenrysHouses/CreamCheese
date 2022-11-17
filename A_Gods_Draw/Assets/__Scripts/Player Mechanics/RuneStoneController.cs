using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStoneController : MonoBehaviour
{
    [SerializeField] PlayerTracker playerState;
    [SerializeField] Rune[] runes;
    [SerializeField] runeRenderer[] renderers;

    void Update()
    {
        updateRunes(playerState);
    }

    public void ToggleRune(RuneType TargetRune)
    {
        for (int i = 0; i < runes.Length; i++)
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
        for (int i = 0; i < runes.Length; i++)
        {
            if(runes[i].Name != TargetRune)
                continue;
            
            runes[i].State = state;
            break;
        }
    }

    void updateRunes(PlayerTracker player)
    {
        foreach (var rune in runes)
        {
            foreach (var playRune in player.CurrentRunes)
            {
                if(rune.Name.Equals(playRune.Name))
                {
                    switch(playRune.State)
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
}

public enum RuneType
{
    UrrStrength = 0,
    FeWealth = 1,
    TursChaos = 2
}

public enum RuneState
{
    Active,
    Disabled,
    Temporary
}

[System.Serializable]
public class Rune
{
    public int ID => (int)Name;
    public RuneType Name;
    public RuneState State;

    public Rune(RuneType type, RuneState state)
    {
        this.Name = type;
        this.State = state;
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