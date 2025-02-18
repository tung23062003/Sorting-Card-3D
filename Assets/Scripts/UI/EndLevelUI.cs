using UnityEngine;
using UnityEngine.UI;

public class EndLevelUI : MonoBehaviour
{
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Button backMenuBtn;

    private void Awake()
    {
        nextLevelBtn.onClick.AddListener(NextLevel);
        backMenuBtn.onClick.AddListener(BackMenu);
    }

    private void OnDestroy()
    {
        nextLevelBtn.onClick.RemoveAllListeners();
        backMenuBtn.onClick.RemoveAllListeners();
    }

    private void NextLevel()
    {
        SceneController.Instance.ReLoadScene();
    }
    private void BackMenu()
    {
        SceneController.Instance.LoadScene(GameConstants.menuScene);
    }
}
