using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    [SerializeField] private GameCardDataSO gameCard;
    [SerializeField] private PlayerCardDataSO playerCard;
    [SerializeField] private PlayerInfoDataSO playerInfo;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            Debug.Log($"Singleton {this.name} is already exist!!!");
        }

        StartCoroutine(LoadCardsFromJSON("cards_data.json", (allcards) => gameCard.gameCardList = allcards));
        //playerCard.playerCardList = LoadCardsFromJSON("my_card.json", () => playerCard.SetCardsQuanityPerRarity());
        StartCoroutine(LoadPlayerInfoFromJSON("player_info.json", (info) => playerInfo.playerInfo = info));
        //playerInfo.playerInfo = LoadPlayerInfoFromJSON("player_info.json");
    }
    //    public List<CardData> LoadCardsFromJSON(string filePath, Action OnLoadDone = null)
    //    {
    //        string fullPath = Path.Combine(Application.streamingAssetsPath, filePath);
    //#if UNITY_EDITOR
    //        //fullPath = Path.Combine(Application.streamingAssetsPath, filePath);
    //        if (File.Exists(fullPath))
    //        {
    //            string jsonContent = File.ReadAllText(fullPath);
    //            if (string.IsNullOrEmpty(jsonContent))
    //                return null;
    //            var allCards = JsonUtility.FromJson<CardDataWrapper>(jsonContent).cards;


    //            Debug.Log($"Loaded {allCards.Count} cards from {filePath}.");
    //            OnLoadDone?.Invoke();
    //            return allCards;
    //        }
    //        else
    //        {
    //            Debug.LogError($"File not found: {fullPath}");
    //        }


    //#if UNITY_ANDROID
    //        UnityWebRequest request = UnityWebRequest.Get(fullPath);
    //        yield return request.SendWebRequest();

    //        if (request.result == UnityWebRequest.Result.Success)
    //        {
    //            string jsonContent = request.downloadHandler.text;
    //            if (string.IsNullOrEmpty(jsonContent))
    //            {
    //                Debug.LogError("JSON content is empty.");
    //                yield break;
    //            }

    //            var allCards = JsonUtility.FromJson<CardDataWrapper>(jsonContent).cards;
    //            Debug.Log($"Loaded {allCards.Count} cards from {filePath}.");
    //            OnLoadDone?.Invoke();
    //        }
    //        else
    //        {
    //            Debug.LogError($"Failed to load file: {fullPath}. Error: {request.error}");
    //        }
    //#endif
    //        return null;
    //    }

    public IEnumerator LoadCardsFromJSON(string filePath, Action<List<CardData>> OnLoadDone = null)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, filePath);

#if UNITY_EDITOR || UNITY_STANDALONE
        if (File.Exists(fullPath))
        {
            string jsonContent = File.ReadAllText(fullPath);
            if (string.IsNullOrEmpty(jsonContent))
            {
                Debug.LogError("JSON content is empty.");
                OnLoadDone?.Invoke(null);
                yield break;
            }

            var allCards = JsonUtility.FromJson<CardDataWrapper>(jsonContent).cards;
            Debug.Log($"Loaded {allCards.Count} cards from {filePath}.");
            OnLoadDone?.Invoke(allCards);
        }
        else
        {
            Debug.LogError($"File not found: {fullPath}");
            OnLoadDone?.Invoke(null);
        }
#elif UNITY_ANDROID
        UnityWebRequest request = UnityWebRequest.Get(fullPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonContent = request.downloadHandler.text;
            if (string.IsNullOrEmpty(jsonContent))
            {
                Debug.LogError("JSON content is empty.");
                OnLoadDone?.Invoke(null);
                yield break;
            }

            var allCards = JsonUtility.FromJson<CardDataWrapper>(jsonContent).cards;
            Debug.Log($"Loaded {allCards.Count} cards from {filePath}.");
            OnLoadDone?.Invoke(allCards);
        }
        else
        {
            Debug.LogError($"Failed to load file: {fullPath}. Error: {request.error}");
            OnLoadDone?.Invoke(null);
        }
#else
        Debug.LogError("Platform not supported for StreamingAssets loading.");
        OnLoadDone?.Invoke(null);
#endif

    }

    private IEnumerator LoadPlayerInfoFromJSON(string filePath, Action<PlayerInfo> onLoaded)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, filePath);

#if UNITY_ANDROID
        UnityWebRequest request = UnityWebRequest.Get(fullPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonContent = request.downloadHandler.text;

            if (!string.IsNullOrEmpty(jsonContent))
            {
                PlayerInfo playerInfo = JsonUtility.FromJson<PlayerInfo>(jsonContent);
                Debug.Log($"Loaded PlayerInfo from {filePath}.");
                onLoaded?.Invoke(playerInfo);
            }
            else
            {
                Debug.LogError($"Failed to parse PlayerInfo from empty content in {filePath}");
            }
        }
        else
        {
            Debug.LogError($"Failed to load PlayerInfo from {filePath}. Error: {request.error}");
        }
#else
    if (File.Exists(fullPath))
    {
        string jsonContent = File.ReadAllText(fullPath);
        if (!string.IsNullOrEmpty(jsonContent))
        {
            PlayerInfo playerInfo = JsonUtility.FromJson<PlayerInfo>(jsonContent);
            Debug.Log($"Loaded PlayerInfo from {filePath}.");
            onLoaded?.Invoke(playerInfo);
        }
        else
        {
            Debug.LogError($"Failed to parse PlayerInfo from empty content in {filePath}");
        }
    }
    else
    {
        Debug.LogError($"File not found: {fullPath}");
    }
#endif
    }

    public void SaveDataToJSON()
    {
        string playerCardPath = Path.Combine(Application.streamingAssetsPath, "my_card.json");
        SaveCardsToJSON(playerCard.playerCardList, playerCardPath);

        string playerInfoPath = Path.Combine(Application.streamingAssetsPath, "player_info.json");
        SavePlayerInfoToJSON(playerInfo, playerInfoPath);
    }

    private void SaveCardsToJSON(List<CardData> cards, string filePath)
    {
        if (cards != null)
        {
            string json = JsonUtility.ToJson(new CardDataWrapper { cards = cards }, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"Saved {cards.Count} cards to {filePath}.");
        }
    }

    private void SavePlayerInfoToJSON(PlayerInfoDataSO playerInfoData, string filePath)
    {
        if (playerInfoData != null)
        {
            string json = JsonUtility.ToJson(playerInfoData.playerInfo, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"Saved player info to {filePath}.");
        }
    }

    private void OnApplicationQuit()
    {
        SaveDataToJSON();
    }
}
