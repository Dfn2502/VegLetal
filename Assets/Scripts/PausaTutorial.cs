using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorPausaTutorial : MonoBehaviour
{
    public GameObject panelPausa;
    public Text textoConsejos;

    public string[] listaConsejos =
    {
        "Consejo: Presiona exactamente las teclas indicadas.",
        "Consejo: Algunas rondas requieren dos teclas al mismo tiempo.",
        "Consejo: El tutorial no avanza hasta acertar.",
        "Consejo: Puedes pausar el tutorial con ESC."
    };

    private bool juegoPausado = false;

    void Start()
    {
        if (panelPausa != null)
            panelPausa.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        juegoPausado = true;

        if (panelPausa != null)
            panelPausa.SetActive(true);

        MostrarConsejoAleatorio();

        Time.timeScale = 0f;
    }

    public void Reanudar()
    {
        juegoPausado = false;

        if (panelPausa != null)
            panelPausa.SetActive(false);

        Time.timeScale = 1f;
    }

    void MostrarConsejoAleatorio()
    {
        if (textoConsejos != null && listaConsejos.Length > 0)
        {
            int random = Random.Range(0, listaConsejos.Length);
            textoConsejos.text = listaConsejos[random];
        }
    }

    public void ReiniciarTutorial()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        CambioEscena.Instance.CambiarEscena("EscenaInicio");
    }
}