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
        int totalDiff = difference;
        if(Health + difference > MaxHealth)
            totalDiff = MaxHealth - Health;

        Health = Mathf.Clamp(Health + difference, 0, MaxHealth);
        HealthChanges.Add(totalDiff);
    }

    public void resetHealth()
    {
        UpdateHealth(MaxHealth);
    }

    public void setDeck(DeckListData deckData)
    {
        CurrentDeck.deckData = deckData;
    }

    public void addRune(rune rune)
    {
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

    public void resetTurnRunes(CombatState trigger)
    {
        foreach (var rune in CurrentRunes)
        {
            if(rune.RuneData.Trigger.Equals(trigger))
                rune.resetTurnTrigger();
        }
    }

    public void resetGameRunes(CombatState trigger)
    {
        foreach (var rune in CurrentRunes)
        {
            if(rune.RuneData.Trigger.Equals(trigger))
                rune.resetGameTrigger();
        }
    }
}