using UnityEngine;

public class VictoryPoint : MonoBehaviour
{
    [Header("Victory Messages Settings")]
    [SerializeField] private string victoryMessage = "Hider reached the goal!";
    [SerializeField] private float victoryMessageDuration = 2f;
    [SerializeField] private string victoryEndMessage = "Hider escaped!";

    private bool gameEnded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameEnded) return;

        if (other.CompareTag("Hider"))
        {
            GameMediator.Instance.GetHUD().DisplayMessage(victoryMessage, victoryMessageDuration);
            GameData.GameResult = victoryEndMessage;
            GameManager.Instance.EndGame(GameData.GameResult);
            gameEnded = true;
        }
    }
}