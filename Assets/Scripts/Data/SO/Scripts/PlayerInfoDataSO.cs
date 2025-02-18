using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Player Info Data", menuName = "Scriptable Object/Player Info Data")]
public class PlayerInfoDataSO : ScriptableObject
{
    public PlayerInfo playerInfo;

    public void NextLevel(int i = 1)
    {
        playerInfo.level += i;
    }
    public void ResetSO()
    {
        playerInfo = null;
    }
}

[Serializable]
public class PlayerInfo
{
    public string name;
    public int level;
    public int coin;
    public int gem;
}
