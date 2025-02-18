using System;
using UnityEngine;
using UnityEngine.Events;

public static class GameConstants
{
    public static string card = "Card";
    public static string cardStack = "CardStack";
    public static string coin = "Coin";

    public static string menuScene = "MainMenu";
    public static string mainScene = "MainScene";

    public static string Common = "Common";
    public static string Rare = "Rare";
    public static string Epic = "Epic";
    public static string Legendary = "Legendary";


    public static string red = "Red";
    public static string green = "Green";
    public static string blue = "Blue";
    public static string yellow = "Yellow";

}

public static class GameEvent
{
    public static Action OnCardMove;
    public static Action<int> OnCardScore;
    public static Action OnWinLevel;

}

public enum CardColor
{
    Red,
    Green,
    Blue,
    Yellow
}

public enum CardRarity { Common, Rare, Epic, Legendary }

public enum CardRarityCanOpen { Common, Rare, Epic, Legendary }

public static class Config
{
    public static int totalColor = 5;
    public static int rarePackRequired = 3;
    public static int epicPackRequired = 2;
    public static int legendPackRequired = 1;
}
