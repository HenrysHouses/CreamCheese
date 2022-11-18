/* 
 * Written by 
 * Henrik
*/

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerTracker")]
public class PlayerTracker : ScriptableObject
{
    public int Health;
    public int MaxHealth;
    [HideInInspector] public List<int> HealthChanges = new List<int>();
    public DeckList_SO CurrentDeck;

    // player's runes here
    [SerializeField] private List<RuneData> _runeData = new List<RuneData>();
    public List<rune> CurrentRunes = new List<rune>();

    private void OnEnable() {
        _runeData.Clear();
    }

    public void UpdateHealth(int difference)
    {
        Health += difference;
        HealthChanges.Add(difference);
    }

    public void resetHealth()
    {
        Health = MaxHealth;
    }

    public void setDeck(DeckListData deckData)
    {
        List<Card_SO> newDeck = new List<Card_SO>();
        for (int i = 0; i < deckData.deckListData.Count; i++)
        {
            newDeck.Add(deckData.deckListData[i]);
        }

        CurrentDeck.SetDeck(newDeck);
    }

    public void addRune(rune rune)
    {
        Debug.Log(CurrentRunes.Count);
        Debug.Log(_runeData.Count);

        foreach (var runesData in _runeData)
        {
            if(runesData.Name == rune.RuneData.Name)
                return;
        }

        CurrentRunes.Add(rune);
        
        foreach (var _rune in CurrentRunes)
        {
            _runeData.Add(_rune.RuneData);
        }
    }

    public void triggerRune(TurnController controller, CombatState trigger)
    {
        foreach (var rune in CurrentRunes)
        {
            if(rune.RuneData.Trigger.Equals(trigger))
                rune.RuneEffect(controller);
        }
    }

    public void resetRune(TurnController controller, CombatState trigger)
    {
        foreach (var rune in CurrentRunes)
        {
            if(rune.RuneData.Trigger.Equals(trigger))
                rune.resetTrigger();
        }
    }
}