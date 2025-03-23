using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public CharactersSO[] availableCharacters; // Assign 3 character SOs in Inspector
    public Button[] characterButtons; // Assign 3 buttons in Inspector
    public GameObject characterSelectionPanel;
    private static CharactersSO selectedCharacter; // Store the character across scenes

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

        // Store character choice for next scene
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacter.characterName);

        Debug.Log("Game Started with " + selectedCharacter.characterName);
        characterSelectionPanel.SetActive(false); // Hide selection UI

        // Load the game scene
        SceneManager.LoadScene("Game_one");
    }

    public static CharactersSO GetSelectedCharacter()
    {
        return selectedCharacter;
    }
}
