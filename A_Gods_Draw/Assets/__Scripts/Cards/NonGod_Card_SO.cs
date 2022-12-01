using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "ScriptableObjects/Non-God card"), System.Serializable]
public class NonGod_Card_SO : Card_SO
{
    //public Sprite icon;
    [HideInInspector]
    public int cardStrenghIndex;
    public List<ActionsForTarget> targetActions = new();
    public GodActionEnum correspondingGod;

    private void OnValidate()
    {
        cardStrenghIndex = 0;
        if (targetActions.Count == 0)
            return;

        foreach (ActionsForTarget a in targetActions)
        {
            a.SetNTar(1);
        }

        if (type == CardType.Attack)
        {
            while (targetActions[0][cardStrenghIndex].actionEnum != CardActionEnum.Attack)
            {
                if (cardStrenghIndex == targetActions.Count - 1)
                    break;
                cardStrenghIndex++;
            }
        }
        else if (type == CardType.Defence)
        {
            while (targetActions[0][cardStrenghIndex].actionEnum != CardActionEnum.Defend)
            {
                if (cardStrenghIndex == targetActions.Count - 1)
                    break;
                cardStrenghIndex++;
            }
        }
        else if (type == CardType.Buff)
        {
            while (targetActions[0][cardStrenghIndex].actionEnum != CardActionEnum.Buff)
            {
                if (cardStrenghIndex == targetActions.Count - 1)
                    break;
                cardStrenghIndex++;
            }
        }
        else
        {
            cardStrenghIndex = 0;
        }
    }
}

[System.Serializable]
public struct CardActionData
{
    public CardActionEnum actionEnum;
    public int actionStrength;
    public bool PlayOnPlacedOrTriggered_SFX;
    public EventReference action_SFX;
    public ActionVFX _VFX;
}


[System.Serializable]
public struct ActionsForTarget
{
    public int numOfTargets;
    public List<CardActionData> targetActions;

    public void SetNTar(int a)
    {
        numOfTargets = a;
    }

    public CardActionData this[int key]
    {
        get => targetActions[key];
        set => targetActions[key] = value;
    }

    public int Count => targetActions.Count;
}
