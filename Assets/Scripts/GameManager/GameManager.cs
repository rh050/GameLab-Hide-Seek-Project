using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CharactersSO[] characterList; // Assign all characters in Inspector
    public Transform spawnPoint; // Assign a spawn point in the game scene

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

    //private void SpawnSelectedCharacter()
    //{
    //    string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter", "");
    //    if (string.IsNullOrEmpty(selectedCharacterName))
    //    {
    //        Debug.LogError("No character selected! Returning to Main Menu...");
    //        SceneManager.LoadScene("MainMenu");
    //        return;
    //    }

    //    CharactersSO selectedCharacter = null;
    //    foreach (var character in characterList)
    //    {
    //        if (character.characterName == selectedCharacterName)
    //        {
    //            selectedCharacter = character;
    //            break;
    //        }
    //    }

    //    if (selectedCharacter == null)
    //    {
    //        Debug.LogError("Character not found in list!");
    //        return;
    //    }

    //    GameObject player = Instantiate(selectedCharacter.characterPrefab, spawnPoint.position, Quaternion.identity);
    //    Debug.Log("Spawned " + selectedCharacter.characterName + " at " + spawnPoint.position);
    //}


    private void SpawnSelectedCharacter()
    {
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter", "");
        if (string.IsNullOrEmpty(selectedCharacterName))
        {
            Debug.LogError("No character selected! Returning to Main Menu...");
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
            Debug.LogError("Character not found in list!");
            return;
        }

        
        GameObject player = Instantiate(selectedCharacter.characterPrefab, spawnPoint.position, Quaternion.identity);

        Hider hider = player.GetComponent<Hider>();
        if (hider != null)
        {
            hider.AssignCharacter(selectedCharacter);
            Debug.Log("Assigned Character to Hider: " + selectedCharacter.characterName);
        }
        else
        {
            Debug.LogError("Hider component missing on player prefab!");
        }

        Debug.Log("Spawned " + selectedCharacter.characterName + " at " + spawnPoint.position);
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
        if (gameStarted && gameTime > 0)
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
        GameData.GameResult = result;
        Debug.Log(result);
        SceneManager.LoadScene("GameOverScene");
    }

    public bool GetGameStarted() => gameStarted;
    public float GetGameTime() => gameTime;
}
