using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TurnManager;

public abstract class Card_Behaviour : MonoBehaviour
{
    protected Card_SO card;
    protected TurnManager manager;

    protected bool played = false;

    public readonly bool isReady = false;

    public virtual void Initialize(Card_SO card)
    {
        this.card = card;
    }

    public void SetManager(TurnManager manager)
    {
        this.manager = manager;
    }

    public virtual void OnClick()
    {
        if (!played)
        {
            manager.SelectedCard(this);
            played = true;
        }
    }

    public Card_SO GetCardSO() { return card; }
    // public void setCardSO(Card_SO Stats) { card = Stats; }

    public virtual IEnumerator OnPlay(List<IMonster> enemies, List<LaneInfo> currLane, PlayerController player, God_Behaviour god)
    {
        yield return new WaitUntil(() => { return true; });
        manager.FinishedPlay(this);
    }
    public virtual void OnAction() { }
}
