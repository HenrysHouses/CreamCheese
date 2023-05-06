using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HH.PropertyAttributes;

[CreateAssetMenu(fileName = "Item Pool", menuName = "A_Gods_Draw/Item Pool", order = 2)]
public class ItemPool_ScriptableObject : ScriptableObject 
{
    [SerializeField, ReadOnly] float _TotalWeights;
    [field:SerializeField] public ItemDrop[] Items;

    public Object getDroppedItem(bool contentIsText)
    {
        float _RolledItem = Random.Range(0f, _TotalWeights);

        for (int i = 0; i < Items.Length; i++)
        {
            if(Items[i].UpperRange < _RolledItem)
                continue;

            return Items[i].Item;
        }
        return null;
    }

    public Object getDroppedItem(Object[] Excluding = null, bool contentIsText = false)
    {
        // Create loot pool with excluded items
        ItemPool_ScriptableObject excludedLootPool = Clone();
        List<ItemDrop> itemDrops = new List<ItemDrop>();
        
        for (int i = 0; i < excludedLootPool.Items.Length; i++)
        {
            bool shouldSkip = false;
            for (int j = 0; j < Excluding.Length; j++)
            {
                if(!contentIsText)
                {
                    if(excludedLootPool.Items[i].Item.Equals(Excluding[j]))
                    {
                        shouldSkip = true;
                        break;
                    }
                }
                else
                {
                    string targetText = excludedLootPool.Items[i].Item.name;
                    string excludedText = Excluding[j].GetType().ToString();

                    if(targetText.Contains(excludedText))
                    {
                        shouldSkip = true;
                        break;
                    }
                }

                if(j == Excluding.Length-1 && !shouldSkip)
                    itemDrops.Add(excludedLootPool.Items[i]);
            }

            if(shouldSkip)
                continue;

            if(Excluding.Length == 0)
                itemDrops.Add(excludedLootPool.Items[i]);
        }

        excludedLootPool.Items = itemDrops.ToArray();
        excludedLootPool.updateTotalWeights();
        excludedLootPool.updateItemDropRanges();

        // Roll dropped item
        float _RolledItem = Random.Range(0f, excludedLootPool._TotalWeights);

        for (int i = 0; i < excludedLootPool.Items.Length; i++)
        {
            if(excludedLootPool.Items[i].UpperRange < _RolledItem)
                continue;

            Object droppedItem = excludedLootPool.Items[i].Item;
            return droppedItem;
        }
        return null;
    }
    
    private void OnValidate() 
    {
        updateTotalWeights();
        updateItemDropRanges();

        // testDropChances();
    }

    public void updateTotalWeights() 
    {
        float total = 0;
        if(Items == null)
        {
            _TotalWeights = 0;
            return;
        }

        for (int i = 0; i < Items.Length; i++)
        {
            total += Items[i].Weight;
        }

        if(total == 0)
            Debug.LogWarning(this.name + "'s loot pool total weights totals to 0."); // ? not sure why the name is not applied

        _TotalWeights = total;
    }

    void updateItemDropRanges()
    {
        float CurrentWeight = 0;

        if(Items == null)
            return;

        for (int i = 0; i < Items.Length; i++)
        {
            float TopRange = CurrentWeight + Items[i].Weight;

            if(i == Items.Length-1 || !(TopRange == 1 && Items[0].Weight == 0))
                Items[i].UpperRange = TopRange;
            else
                Items[i].UpperRange = TopRange-1;
            CurrentWeight += Items[i].Weight;
            Items[i].updateDropChance(_TotalWeights);
        }
    }

    private ItemPool_ScriptableObject Clone()
    {
        ItemPool_ScriptableObject _Clone = ScriptableObject.CreateInstance<ItemPool_ScriptableObject>();
        _Clone.name = this.name + " (Clone)";
        _Clone._TotalWeights = _TotalWeights;
        
        _Clone.Items = new ItemDrop[Items.Length]; 
        for (int i = 0; i < Items.Length; i++)
        {
            _Clone.Items[i] = Items[i].Clone();
        }
        return _Clone;
    }
    
    
    // [SerializeField] Object testExclusion;
    // private void testDropChances()
    // {

    //     // Debug.Log("Item Roll: " + itemRoll + " -> " + getDroppedItem<ScriptableObject>(itemRoll));
    //     Debug.Log("Item Roll: " + getDroppedItem<ScriptableObject>(new Object[]{testExclusion}));
    // }
}