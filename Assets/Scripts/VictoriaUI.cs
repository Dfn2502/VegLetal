using UnityEngine;
using UnityEngine.UI;

public class VictoriaUI : MonoBehaviour
{
    public Text textoScore;
    public CambioEscena cambioEscena;

    void Start()
    {
        int score = PlayerPrefs.GetInt("ScoreFinal", 0);

        Debug.Log("SCORE EN VICTORIA: " + score);

        textoScore.text = "Score Final: " + score;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CambioEscena.Instance.CambiarEscena("Creditos");
        }
    }
}
