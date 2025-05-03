using UnityEngine;

public class VictoryPoint : MonoBehaviour
{
    private bool gameEnded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameEnded) return;

        if (other.CompareTag("Hider"))
        {
            GameMediator.Instance.GetHUD().DisplayMessage("Hider reached the goal!", 2f);
            GameData.GameResult = "Hider escaped!";
            GameManager.Instance.EndGame(GameData.GameResult);
            gameEnded = true;
        }
    }
}