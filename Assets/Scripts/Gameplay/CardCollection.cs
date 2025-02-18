using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardCollection : Singleton<CardCollection>
{
    public List<CardData> myCards = new();


    private void Start()
    {
        //myCards = GameData.Instance.LoadCardsFromJSON("my_card.json");
    }
    
    
    
}
