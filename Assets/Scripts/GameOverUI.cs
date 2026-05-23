using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Text textoScore;

    void Start()
    {
        int score = PlayerPrefs.GetInt("ScoreFinal", 0);
        textoScore.text = "Score: " + score;
    }
}
