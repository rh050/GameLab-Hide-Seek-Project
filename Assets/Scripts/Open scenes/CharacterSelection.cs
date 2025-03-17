using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public CharactersSO[] availableCharacters; // Assign 3 character SOs in Inspector
    public Button[] characterButtons; // Assign 3 buttons in Inspector
    public GameObject characterSelectionPanel;
    private CharactersSO selectedCharacter;

    void Start()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i;
            characterButtons[i].onClick.AddListener(() => SelectCharacter(index));
        }
    }

    void SelectCharacter(int index)
    {
        selectedCharacter = availableCharacters[index];
        Debug.Log("Selected Character: " + selectedCharacter.characterName); // Check in Console
    }

    public void ConfirmSelection()
    {
        if (selectedCharacter == null)
        {
            Debug.LogWarning("No character selected!"); // Ensure a character is selected
            return;
        }

        // Instantiate the selected character's prefab
        GameObject player = Instantiate(selectedCharacter.characterPrefab, Vector3.zero, Quaternion.identity);
        Hider hiderComponent = player.GetComponent<Hider>();

        if (hiderComponent != null)
        {
            hiderComponent.AssignCharacter(selectedCharacter);
        }

        Debug.Log("Game Started with " + selectedCharacter.characterName);
        characterSelectionPanel.SetActive(false); // Hide selection UI

        // Load the game after confirming character selection
        SceneManager.LoadScene("Game_one");
    }
}
