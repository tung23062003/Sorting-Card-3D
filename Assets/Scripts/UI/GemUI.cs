using TMPro;
using UnityEngine;

public class GemUI : MonoBehaviour
{
    [SerializeField] private PlayerInfoDataSO playInfo;
    [SerializeField] private TextMeshProUGUI gemTxt;

    private void Start()
    {
        gemTxt.text = playInfo.playerInfo.gem.ToString("0");
    }
}
