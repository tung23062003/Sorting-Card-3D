using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Card Data", menuName = "Scriptable Object/Game Card Data", order = 0)]
public class GameCardDataSO : ScriptableObject
{
    public List<CardData> gameCardList = new();


    public void ResetSO()
    {
        gameCardList.Clear();
    }
}


[Serializable]
public class CardDataWrapper
{
    public List<CardData> cards;
}

[Serializable]
public class CardData
{
    public int index;
    public string cardName;
    public string cardColor;
    public string rarity;
    public string imagePath;

    public override bool Equals(object obj)
    {
        if (obj is CardData other)
        {
            return index == other.index &&
                   cardName == other.cardName &&
                   cardColor == other.cardColor &&
                   rarity == other.rarity &&
                   imagePath == other.imagePath;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(index, cardName, cardColor, rarity, imagePath);
    }
}