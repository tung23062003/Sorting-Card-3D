using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Player Card Data", menuName = "Scriptable Object/Player Card Data", order = 1)]
public class PlayerCardDataSO : ScriptableObject
{
    public List<CardData> playerCardList = new();
    public CardQuantity cardQuantity;

    public void AddPlayerCards(List<CardData> cards)
    {
        if(playerCardList == null || playerCardList.Count == 0)
            playerCardList = cards;
        else
            playerCardList.AddRange(cards);
    }

    public CardData GetPlayerCardByName(string name)
    {
        return playerCardList.Find(item => item.cardName == name);
    }

    public List<CardData> GetPlayerCardsByRarity(string rarity)
    {
        return playerCardList.FindAll(item => item.rarity == rarity);
    }

    public void SetCardsQuanityPerRarity()
    {
        cardQuantity.commonQuantity = GetPlayerCardsByRarity(GameConstants.Common).Count;
        cardQuantity.rareQuantity = GetPlayerCardsByRarity(GameConstants.Rare).Count;
        cardQuantity.epicQuantity = GetPlayerCardsByRarity(GameConstants.Epic).Count;
        cardQuantity.legendQuantity = GetPlayerCardsByRarity(GameConstants.Legendary).Count;
    }

    public void ResetSO()
    {
        playerCardList.Clear();
    }
}

[Serializable]
public class CardQuantity
{
    public int commonQuantity;
    public int rareQuantity;
    public int epicQuantity;
    public int legendQuantity;
}