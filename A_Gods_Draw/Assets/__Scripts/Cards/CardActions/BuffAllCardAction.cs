using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAllCardAction : CardAction
{
    List<GameObject> SpawnedCoins = new List<GameObject>();

    void SpawnCoins(int amount, ActionCard_Behaviour card)
    {
        for (int i = 0; i < amount; i++)
        {
            // Debug.Log("spawning coin");
            GameObject spawn  = GameObject.Instantiate(Resources.Load<GameObject>("Prop_Coin_PRE_v1"));
            SpawnedCoins.Add(spawn);
            spawn.transform.SetParent(card.RootTransform, false);
            Vector3 offsetPos = spawn.transform.localPosition;
            offsetPos.x += Random.Range(-0.0262f, 0.0262f);
            offsetPos.y += Random.Range(-0.09240003f, 0.09240003f);
            offsetPos.z += -1.2137f + ((-1.2137f/amount) * i);
            spawn.transform.localPosition = offsetPos;
        }
        playSFX(card.gameObject);
    }

    public override IEnumerator OnAction(BoardStateController _board, ActionCard_Behaviour _source)
    {
        isReady = true;
        yield break;
    }

    public override void CardPlaced(BoardStateController _board, ActionCard_Behaviour _source)
    {
        playSFX(_source.gameObject);

        foreach(ActionCard_Behaviour _card in _board.placedCards)
        {

            if(_card.GetCardType == CardType.Attack)
            {
                _card.Buff(_source.stats.strength, false);
                SpawnCoins(_source.stats.strength, _card);
                _card.UpdateQueuedDamage(true);
            }
        }
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "", 0);
        Debug.LogError("Buff All has no VFX");
    }
}
