using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Non-God card"), System.Serializable]
public class NonGod_Card_SO : Card_SO
{
    //public Sprite icon;
    [HideInInspector]
    public int cardStrenghIndex;
    public List<CardActionData> cardActions;
    public GodActionEnum correspondingGod;

    private void OnValidate()
    {
        cardStrenghIndex = 0;
        if (type == CardType.Attack)
        {
            while (cardActions[cardStrenghIndex].actionEnum != CardActionEnum.Attack)
            {
                cardStrenghIndex++;
            }
        }
        else if (type == CardType.Defence)
        {
            while (cardActions[cardStrenghIndex].actionEnum != CardActionEnum.Defend)
            {
                cardStrenghIndex++;
            }
        }
        else if (type == CardType.Buff)
        {
            while (cardActions[cardStrenghIndex].actionEnum != CardActionEnum.Buff)
            {
                cardStrenghIndex++;
            }
        }
        else if (type == CardType.Special)
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
}
