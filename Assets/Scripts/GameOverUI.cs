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
        CambioEscena.Instance.CambiarEscena("EscenaJuego");
    }

    public void VolverAlMenu()
    {
        CambioEscena.Instance.CambiarEscena("EscenaInicio");
    }
}
