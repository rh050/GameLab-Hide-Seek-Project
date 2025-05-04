using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Difficulty { Easy, Medium, Hard }

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Difficulty UI")]
    public GameObject difficultyPanel;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        difficultyPanel.SetActive(false);

        easyButton.onClick.AddListener(() => SelectDifficulty(Difficulty.Easy));
        mediumButton.onClick.AddListener(() => SelectDifficulty(Difficulty.Medium));
        hardButton.onClick.AddListener(() => SelectDifficulty(Difficulty.Hard));
    }

    public void ShowDifficultyPanel()
    {
        difficultyPanel.SetActive(true);
    }

    private void SelectDifficulty(Difficulty diff)
    {
        PlayerPrefs.SetInt("GameDifficulty", (int)diff);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game_one");
    }

    public Difficulty GetDifficulty()
    {
        return (Difficulty)PlayerPrefs.GetInt("GameDifficulty", (int)Difficulty.Medium);
    }
}
