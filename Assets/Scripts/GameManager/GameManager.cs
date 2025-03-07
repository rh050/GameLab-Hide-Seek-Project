using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool gameStarted = false;
    private GameHUDController hud;
    private float gameTime = 100f; 
    private SeekerAI seeker; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        hud = FindObjectOfType<GameHUDController>();
        seeker = FindObjectOfType<SeekerAI>(); // חיפוש המחפש בסצנה
    }

    public void StartGame()
    {
        gameStarted = true;
        if (hud != null)
        {
            hud.DisplayMessage("Start!", 2f);
        }

        foreach (Hider hider in GameMediator.Instance.GetAllHiders())
        {
            PlayerController playerController = hider.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = true;
            }
        }
    }
    
    private void Update()
    {
        if (gameStarted && gameTime > 0) // שימוש ב-gameTime 
        {
            gameTime -= Time.deltaTime;
            
            if (gameTime <= 0)
            {
                EndGame("Time's Up! Hiders Win!");
            }
        }
    }

    public void SetNPCMovement(bool enabled)
    {
        foreach (Hider hider in GameMediator.Instance.GetAllHiders())
        {
            PlayerController playerController = hider.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = enabled;
            }
        }

        if (seeker != null)
        {
            seeker.enabled = enabled;
        }
    }
    
    public SeekerAI GetSeeker() => seeker;

    public void EndGame(string result)
    {
        ScoreManager.Instance.AwardSurvivingHiders();
        Debug.Log(result);
        SceneManager.LoadScene("GameOverScene");
    }

    // get and set for gameStarted
    public bool GetGameStarted() => gameStarted;
    public float GetGameTime() => gameTime;
}
