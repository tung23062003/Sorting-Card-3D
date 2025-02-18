using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Design Data", menuName = "Scriptable Object/Design Data")]
public class DesignDataSO : ScriptableObject
{
    public List<CardVisual> cardVisuals = new();

    public CardVisual GetCardVisual(string name)
    {
        return cardVisuals.Find(item => item.name == name);
    }
}

[Serializable]
public class CardVisual
{
    public string name;
    public Sprite sprite;
}
