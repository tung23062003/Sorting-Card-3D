using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log($"Singleton {this.name} is already exist!!!");
        }
    }

    public void NextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(currentIndex + 1);
        else
            Debug.Log("Ban da pha dao game!!!");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void ReLoadScene()
    {
        SceneManager.LoadSceneAsync(GetCurrentSceneIndex());
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
