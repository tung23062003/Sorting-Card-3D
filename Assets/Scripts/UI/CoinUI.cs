using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private PlayerInfoDataSO playInfo;
    [SerializeField] private TextMeshProUGUI coinTxt;

    private void Start()
    {
        coinTxt.text = playInfo.playerInfo.coin.ToString("0");
    }

    public void UpdateCoin(int coin)
    {
        coinTxt.text = coin.ToString();
    }
}
