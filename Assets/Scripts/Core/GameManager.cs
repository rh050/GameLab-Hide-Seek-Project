using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CharactersSO[] characterList; 
    public Transform[] spawnPoints; 

    [Header("Game Messages Settings")]
    [SerializeField] private string startGameMessage = "Start!";
    [SerializeField] private float startGameMessageDuration = 2f;
    [SerializeField] private string timeUpMessage = "Time's Up! Hiders Win!";

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
        seeker = FindObjectOfType<SeekerAI>();
        SpawnSelectedCharacter();
    }

    private void SpawnSelectedCharacter()
    {
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter", "");
        if (string.IsNullOrEmpty(selectedCharacterName))
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        CharactersSO selectedCharacter = null;
        foreach (var character in characterList)
        {
            if (character.characterName == selectedCharacterName)
            {
                selectedCharacter = character;
                break;
            }
        }

        if (selectedCharacter == null)
        {
            return;
        }

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        GameObject player = Instantiate(selectedCharacter.characterPrefab, spawnPoint.position, Quaternion.identity);

        Hider hider = player.GetComponent<Hider>();
        if (hider != null)
        {
            hider.AssignCharacter(selectedCharacter);
            Debug.Log("Assigned Character to Hider: " + selectedCharacter.characterName);
        }

        Debug.Log("Spawned " + selectedCharacter.characterName + " at " + spawnPoint.position);
    }

    public void StartGame()
    {
        gameStarted = true;
        if (hud != null)
        {
            hud.DisplayMessage(startGameMessage, startGameMessageDuration);
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
        if (gameStarted && gameTime > 0)
        {
            gameTime -= Time.deltaTime;

            if (gameTime <= 0)
            {
                EndGame(timeUpMessage);
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


    public void EndGame(string result)
    {
        ScoreManager.Instance.AwardSurvivingHiders();
        GameData.GameResult = result;
        Debug.Log(result);
        SceneManager.LoadScene("GameOverScene");
    }

    public bool GetGameStarted() => gameStarted;
    public float GetGameTime() => gameTime;
}
