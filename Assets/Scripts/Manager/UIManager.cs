using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject winLevelPanel;
    [SerializeField] private PlayerInfoDataSO playerInfo;
    [SerializeField] private CoinUI coinUI;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private Button backBtn;

    protected override void Awake()
    {
        base.Awake();
        GameEvent.OnCardScore += UpdateScore;
        GameEvent.OnWinLevel += OpenWinPanel;
        backBtn.onClick.AddListener(HandleBackBtn);
    }

    private void OnDestroy()
    {
        GameEvent.OnCardScore -= UpdateScore;
        GameEvent.OnWinLevel -= OpenWinPanel;
        backBtn.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        levelTxt.text = "Level " + playerInfo.playerInfo.level.ToString();
    }

    private void UpdateScore(int bonusCoin)
    {
        playerInfo.playerInfo.coin += bonusCoin;
        coinUI.UpdateCoin(playerInfo.playerInfo.coin);
    }

    private void OpenWinPanel()
    {
        winLevelPanel.SetActive(true);
    }

    private void HandleBackBtn()
    {
        SceneController.Instance.LoadScene(GameConstants.menuScene);
    }
}
