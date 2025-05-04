using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public CharactersSO[] availableCharacters;     
    public Button[] characterButtons;               
    public GameObject characterSelectionPanel;     

    private CharactersSO selectedCharacter;

    void Start()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int idx = i;
            characterButtons[i].onClick.AddListener(() =>
            {
                selectedCharacter = availableCharacters[idx];
                PlayerPrefs.SetString("SelectedCharacter", selectedCharacter.characterName);
                PlayerPrefs.Save();

                characterSelectionPanel.SetActive(false);
                DifficultyManager.Instance.ShowDifficultyPanel();
            });
        }
    }

    // אם צריך בגישה סטטית:
    public static CharactersSO GetSelectedCharacter()
    {
        string name = PlayerPrefs.GetString("SelectedCharacter", "");
        return null; 
    }
}
