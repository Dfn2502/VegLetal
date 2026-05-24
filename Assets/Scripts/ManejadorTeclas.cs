using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ManejadorTeclas : MonoBehaviour
{
    private bool yaGano = false;
    public bool enPelea = false;
    public bool enPantallaEspecial = false;

    public int puntaje = 0;
    public Text textoPuntaje;

    private Coroutine blinkContinuar;
    private Coroutine blinkSalir;
    public GameObject prefabBase;
    public Sprite[] spritesTeclas; 
    public Transform[] puntosAparicion;
    public float tiempoReaccion = 1.0f;
    public CorazonUI corazonUI;

    public ControladorPausa controladorPausa;

    public Image panelOscuro;
    public CanvasGroup imgContinuar;
    public CanvasGroup imgSalir;

    public float velocidadParpadeo = 2f;

    public AudioClip[] sonidos;
    public AudioSource audioSource;

    private List<GameObject> teclasActivas = new List<GameObject>();

    private List<KeyCode> teclasGanadoras = new List<KeyCode>();
    private List<GameObject> teclasCorrectas = new List<GameObject>();


    public Jugador jugador;
    public Vector3 posicion;
    public Vector3 posicionEnemigo;

    public GameObject[] enemigosPrefab;
    private int indiceEnemigoActual = 0;
    private Champiñon enemigo;
    public Transform puntoSpawnEnemigo;

    private KeyCode[] teclas = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };

    void Start()
    {
        posicion = new Vector3(-1.05f,-0.75f,-1.16f);
        posicionEnemigo = new Vector3(0.955f, -0.75f, -1.10186f);

        jugador.transform.position = posicion;

        panelOscuro.color = new Color(0, 0, 0, 0);
        CargarSiguienteEnemigo();
        puntaje = 0;
        ActualizarUI();
        corazonUI.MostrarCorazonesJugador(3);

        StartCoroutine(IntroJuego());
    }
    void SumarPunto()
    {
        puntaje++;
        ActualizarUI();
    }
    void ActualizarUI()
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = "Score: " + puntaje;
        }
    }

    IEnumerator IntroJuego()
    {
        enPantallaEspecial = true;

        blinkContinuar = StartCoroutine(Parpadear(imgContinuar));
        blinkSalir = StartCoroutine(Parpadear(imgSalir));

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CambioEscena.Instance.CambiarEscena("EscenaInicio");
            }

            yield return null;
        }

        StopCoroutine(blinkContinuar);
        StopCoroutine(blinkSalir);

        imgContinuar.alpha = 0;
        imgSalir.alpha = 0;

        enPantallaEspecial = false;
        enPelea = true;

        StartCoroutine(CicloDeJuego());
    }
    IEnumerator Parpadear(CanvasGroup canvas)
    {
        while (true)
        {
            while (canvas.alpha < 1f)
            {
                canvas.alpha += Time.deltaTime * velocidadParpadeo;
                yield return null;
            }

            while (canvas.alpha > 0f)
            {
                canvas.alpha -= Time.deltaTime * velocidadParpadeo;
                yield return null;
            }
        }
    }
    IEnumerator FadePanel(float inicio, float fin, float velocidad)
    {
        float t = 0f;

        Color color = panelOscuro.color;

        while (t < 1f)
        {
            t += Time.deltaTime * velocidad;

            float alpha = Mathf.Lerp(inicio, fin, t);

            panelOscuro.color = new Color(
                color.r,
                color.g,
                color.b,
                alpha
            );

            yield return null;
        }

        panelOscuro.color = new Color(
            color.r,
            color.g,
            color.b,
            fin
        );
    }
        IEnumerator MostrarPantallaContinuar()
        {
        enPantallaEspecial = true;

        yield return StartCoroutine(FadePanel(0f, 0.6f, 2f));

        blinkContinuar = StartCoroutine(Parpadear(imgContinuar));
        blinkSalir = StartCoroutine(Parpadear(imgSalir));

        bool continuar = false;

        while (!continuar)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                continuar = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CambioEscena.Instance.CambiarEscena("EscenaInicio");
            }

            yield return null;
        }

        StopCoroutine(blinkContinuar);
        StopCoroutine(blinkSalir);

        imgContinuar.alpha = 0;
        imgSalir.alpha = 0;

        yield return StartCoroutine(FadePanel(0.6f, 0f, 2f));

        enPantallaEspecial = false;
        enPelea = true;
    }
    


    IEnumerator MoverJugador(Vector3 destino, float velocidad)
    {
        while (Vector3.Distance(jugador.transform.position, destino) > 0.01f)
        {
            jugador.transform.position = Vector3.MoveTowards(
                jugador.transform.position,
                destino,
                velocidad * Time.deltaTime
            );

            yield return null;
        }

        jugador.transform.position = destino;
    }
    IEnumerator MoverEnemigo(Vector3 destino, float velocidad)
    {
        while (Vector3.Distance(enemigo.transform.position, destino) > 0.01f)
        {
            enemigo.transform.position = Vector3.MoveTowards(
                enemigo.transform.position,
                destino,
                velocidad * Time.deltaTime
            );

            yield return null;
        }

        enemigo.transform.position = destino;
    }
    IEnumerator CicloDeJuego()
    {
        while (!jugador.estaMuerto)
        {
            yield return StartCoroutine(Ronda());
        }

    }

    IEnumerator Ronda()
    {
        if (yaGano) yield break;

        if (jugador.estaMuerto)
            yield break;

        posicion.x = -1.05f;
        posicionEnemigo.x = 0.955f;
        yield return StartCoroutine(MoverEnemigo(posicionEnemigo, 20f));
        yield return StartCoroutine(MoverJugador(posicion, 20f));
        jugador.Transicion();

        yield return new WaitForSeconds(0.3f);

        audioSource.PlayOneShot(sonidos[2]);
        jugador.Espera();

        GenerarTeclas();

        float tiempoAntesSenal = enemigo.tiempoReaccion * 0.4f;
        yield return new WaitForSeconds(tiempoAntesSenal);

        jugador.Reaccion();
        ActivarSenal();

        float tiempo = 0f;
        bool acierto = false;
        bool inputDetectado = false;
        List<KeyCode> teclasPresionadas = new List<KeyCode>();

        while (tiempo < enemigo.tiempoReaccion && !jugador.estaMuerto)
        {
            foreach (KeyCode tecla in teclas)
            {
                if (Input.GetKeyDown(tecla))
                {
                    inputDetectado = true;

                    if (!teclasPresionadas.Contains(tecla))
                    {
                        teclasPresionadas.Add(tecla);

                        // ✔ Solo suma si esa tecla es correcta
                        if (teclasGanadoras.Contains(tecla))
                        {
                            SumarPunto();
                        }
                    }
                }
            }

            bool todasCorrectas = true;

            foreach (KeyCode tecla in teclasGanadoras)
            {
                if (!teclasPresionadas.Contains(tecla))
                {
                    todasCorrectas = false;
                    break;
                }
            }

            if (todasCorrectas)
            {
                acierto = true;
                break;
            }

            tiempo += Time.deltaTime;
            yield return null;
        }


        posicion.x = 0.9f;
        posicionEnemigo.x = -1.033f;
        yield return StartCoroutine(MoverJugador(posicion, 20f));
        yield return StartCoroutine(MoverEnemigo(posicionEnemigo, 20f));
        jugador.Atacar();
        enemigo.Atacar();
        yield return new WaitForSeconds(2f);
        if (inputDetectado && acierto)
        {
            audioSource.PlayOneShot(sonidos[0]);
            enemigo.RecibirDanio();
            jugador.Victoria();
            corazonUI.QuitarVidaEnemigo();
            if (enemigo.vidasActuales <= 0)
            {
                yield return new WaitForSeconds(2f);

                yield return StartCoroutine(MostrarPantallaContinuar());

                Destroy(enemigo.gameObject);

                CargarSiguienteEnemigo();

            }
            else
            {
                yield return new WaitForSeconds(1f);

                enemigo.Idle();
            
            }
            jugador.Idle();


        }
        else
        {
            audioSource.PlayOneShot(sonidos[1]);
            jugador.RecibirDanio();
            enemigo.Idle();
            yield return new WaitForSeconds(1f);
        }

        LimpiarRonda();

        yield return new WaitForSeconds(0.8f);

    }

    void GenerarTeclas()
    {
        teclasGanadoras.Clear();
        teclasCorrectas.Clear();

        int cantidadCorrectas = Random.Range(1, 3);

        List<int> indicesUsados = new List<int>();

        for(int i=0; i< teclas.Length; i++)
        {
            GameObject nuevaTecla = Instantiate(prefabBase, puntosAparicion[i]);
            nuevaTecla.transform.localPosition = Vector3.zero;

            teclasActivas.Add(nuevaTecla);

            Image img = nuevaTecla.GetComponentInChildren<Image>();
            img.sprite = spritesTeclas[i];

            img.color = Color.white;

            if(indicesUsados.Count < cantidadCorrectas)
            {
                bool seraCorrecta = Random.Range(0,2) == 0;

                if (seraCorrecta)
                {
                    teclasGanadoras.Add(teclas[i]);
                    teclasCorrectas.Add(nuevaTecla);
                    indicesUsados.Add(i);
                }
            }
        }

        if(teclasGanadoras.Count == 0)
        {
            int random = Random.Range(0, teclas.Length);

            teclasGanadoras.Add(teclas[random]);
            teclasCorrectas.Add(teclasActivas[random]);
        }
        
    }

    void ActivarSenal()
    {
        foreach (GameObject t in teclasActivas)
        {
            Image img = t.GetComponentInChildren<Image>();

            if (teclasCorrectas.Contains(t))
            {
                img.color = Color.green;
                t.transform.localScale = Vector3.one * 1.2f;
            }
            else
            {
                img.color = Color.red;
            }
        }
        
    }
    IEnumerator VictoriaFinal()
    {
        enPelea = false;
        enPantallaEspecial = true;

        yield return new WaitForSeconds(1.5f);

        PlayerPrefs.SetInt("ScoreFinal", puntaje);
        PlayerPrefs.Save();

        CambioEscena.Instance.CambiarEscena("Victoria");
    }
    public void CargarSiguienteEnemigo()
    {
        if (indiceEnemigoActual >= enemigosPrefab.Length && !yaGano)
        {
            yaGano = true;
            StopAllCoroutines();
            StartCoroutine(VictoriaFinal());
            return;
        }

        GameObject nuevoEnemigo = Instantiate(
            enemigosPrefab[indiceEnemigoActual],
            puntoSpawnEnemigo.position,
            Quaternion.identity
        );

        enemigo = nuevoEnemigo.GetComponent<Champiñon>();

        if (enemigo == null)
        {
            return;
        }

        corazonUI.MostrarCorazonesEnemigo(enemigo.vidasMaximas);

        indiceEnemigoActual++;
    }

    public void LimpiarRonda()
    {
        foreach (GameObject t in teclasActivas)
        {
            if (t != null) Destroy(t);
        }

        teclasActivas.Clear();
    }
}