using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorPausa : MonoBehaviour
{
    public CambioEscena cambioEscena;
    public GameObject panelPausa;
    public Text textoConsejos;

    public string[] listaConsejos = {
        "Consejo: Mantén la calma, algunos combos requieren presionar dos teclas al mismo tiempo.",
        "Consejo: Observa el color de la señal, atacar antes de tiempo te dejará vulnerable.",
        "Consejo: Cada enemigo tiene tiempos de reacción diferentes. ¡Aprende sus patrones!",
        "Consejo: Si fallas una combinación, recibirás daño de inmediato.",
        "Consejo: Puedes pausar el juego en cualquier momento presionando la tecla ESC."
    };

    private bool juegoPausado = false;

    void Start()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
    }

    public void Reanudar()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void Pausar()
    {
        if (panelPausa != null) panelPausa.SetActive(true);

        MostrarConsejoAleatorio();

        Time.timeScale = 0f;
        juegoPausado = true;
    }

    void MostrarConsejoAleatorio()
    {
        if (textoConsejos != null && listaConsejos.Length > 0)
        {
            int indiceRandom = Random.Range(0, listaConsejos.Length);
            textoConsejos.text = listaConsejos[indiceRandom];
        }
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f; 
        CambioEscena.Instance.CambiarEscena("EscenaInicio");
    }
}