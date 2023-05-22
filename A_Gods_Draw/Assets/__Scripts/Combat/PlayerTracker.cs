/* 
 * Written by 
 * Henrik
*/

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "PlayerTracker")]
public class PlayerTracker : ScriptableObject
{
    public PlayerDataContainer PlayerData;
    public int Health;
    public int MaxHealth;
    [HideInInspector] public List<int> HealthChanges = new List<int>();
    public DeckList_SO CurrentDeck;

    // player's runes here
    [field:SerializeField] public List<RuneData> _runeData {get; private set;} = new List<RuneData>();

    [HideInInspector] public List<rune> CurrentRunes = new List<rune>();

    private void OnEnable() {
        CurrentRunes.Clear();
        PlayerData = LoadPlayerData();
        _runeData = PlayerData.Runes.ToList();
        Health = PlayerData.PlayerHealth;
    }

    public PlayerDataContainer LoadPlayerData()
    {
        PlayerData = GameSaver.LoadPlayerData();
        return PlayerData;
    }

    public void SavePlayerData()
    {

        if(PlayerData == null)
        {

            PlayerData = LoadPlayerData();

        }

        PlayerData.PlayerHealth = Health;
        PlayerData.TimesDefeatedBoss = GameManager.timesDefeatedBoss;
        PlayerData.Runes = _runeData.ToArray();

        GameSaver.SavePlayerData(PlayerData);

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
        
        int lastIndex = CurrentRunes.Count-1;
        _runeData.Add(CurrentRunes[lastIndex].RuneData);
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