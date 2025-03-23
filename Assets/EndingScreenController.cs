using UnityEngine;
using TMPro;

public class EndingScreenController : MonoBehaviour
{
    public TextMeshProUGUI seekerScoreText;
    public TextMeshProUGUI hiderScoreText;
    public TextMeshProUGUI winnerText;
    private void Start()
    {
        // Get scores from GameMediator
        int seekerScore = GameMediator.Instance.GetSeekerScore();
        int hiderScore = GameMediator.Instance.GetHiderScore();

        // Display the scores
        seekerScoreText.text = "Seeker Score: " + seekerScore;
        hiderScoreText.text = "Hider Score: " + hiderScore;
        
        if (seekerScore > hiderScore)
        {
            winnerText.text = "Seeker Is The Winner!";
        }
        else if (seekerScore < hiderScore)
        {
            winnerText.text = "Hider Is The Winner!";
        }
        else
        {
            winnerText.text = "It's a Tie!";
        }
    }
}