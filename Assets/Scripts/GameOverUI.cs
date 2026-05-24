using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Text textoScore;
    public CambioEscena cambioEscena;
    void Start()
    {
        int score = PlayerPrefs.GetInt("ScoreFinal", 0);
        textoScore.text = "Score: " + score;
    }

    public void ReiniciarJuego()
    {
        cambioEscena.CambiarEscena("EscenaJuego");
    }

    public void VolverAlMenu()
    {
        cambioEscena.CambiarEscena("EscenaInicio");
    }
}
