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
#if UNITY_EDITOR
        LoadPlayerData();
#endif
    }

    private void ProcessRunes()
    {

        _runeData.Clear();
        CurrentRunes.Clear();
        _runeData = PlayerData.Runes.ToList();

        foreach(RuneData _data in _runeData)
        {

            switch(_data.Name)
            {
                
                case RuneType.FeWealth:
                WealthRune _tempWealthRune = new WealthRune(1, RuneState.Active);
                _tempWealthRune.RuneData = _data;
                CurrentRunes.Add(_tempWealthRune);
                Debug.Log("Loaded Rune: " + _data.Name);
                break;            

                case RuneType.TursChaos:
                ChaosRune _tempChaosRune = new ChaosRune(1, RuneState.Active);
                _tempChaosRune.RuneData = _data;
                CurrentRunes.Add(_tempChaosRune);
                Debug.Log("Loaded Rune: " + _data.Name);
                break;

                // case RuneType.UrrStrength:
                //     Rune = new StrengthRune(1, RuneState.Active);
                //     break;

                default:
                Debug.Log("this rune has not been implemented: " + _data.Name);
                break;

            }

        }

    }

    public PlayerDataContainer LoadPlayerData()
    {
        PlayerData = GameSaver.LoadPlayerData();
        Health = PlayerData.PlayerHealth;
        ProcessRunes();
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