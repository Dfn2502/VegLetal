using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManejadorTutorial : ManejadorTeclas
{
    public Text textoInstrucciones;


    [Header("PNGs de Interfaz de Fin/Muerte")]
    public GameObject pngEnter; 
    public GameObject pngR;    

    private List<KeyCode> teclasForzadasTutorial = new List<KeyCode>();
    private bool tutorialTerminado = false;
    private bool esperandoEnter = false;
    private bool esperandoR = false;

    new void Start()
    {
        PlayerPrefs.DeleteAll();
        posicion = new Vector3(-1.05f, -0.75f, -1.16f);
        posicionEnemigo = new Vector3(0.955f, -0.75f, -1.10186f);
        jugador.transform.position = posicion;

        if (pngEnter != null) pngEnter.SetActive(false);
        if (pngR != null) pngR.SetActive(false);

        CargarSiguienteEnemigo();
        corazonUI.MostrarCorazonesJugador(3);

        StartCoroutine(FlujoDelTutorial());
    }

    protected override void Update()
    {
        base.Update();

        if (jugador.estaMuerto && !esperandoR)
        {
            StopAllCoroutines();
            base.LimpiarRonda();
            StartCoroutine(MostrarPantallaDerrotaTutorial());
        }

        if (esperandoEnter && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            esperandoEnter = false;
            audioSource.PlayOneShot(sonidos[0]);

            CambioEscena.Instance.CambiarEscena("EscenaJuego");
        }

        if (esperandoR && Input.GetKeyDown(KeyCode.R))
        {
            esperandoR = false;
            audioSource.PlayOneShot(sonidos[0]);

            CambioEscena.Instance.CambiarEscena(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            CambioEscena.Instance.CambiarEscena("EscenaJuego");
        }
    }

    IEnumerator FlujoDelTutorial()
    {
        Champiñon enemigoActual = FindObjectOfType<Champiñon>();

        if (textoInstrucciones != null)
            textoInstrucciones.text = "¡Bienvenido al Tutorial! El juego no avanzará hasta que presiones la tecla correcta.";
        yield return new WaitForSeconds(3.5f);

        teclasForzadasTutorial = new List<KeyCode> { KeyCode.Q };
        if (textoInstrucciones != null)
            textoInstrucciones.text = "¡Presiona la tecla Q para atacar! (Esperando acción...)";
        yield return StartCoroutine(RondaDelTutorialInfinita(enemigoActual));

        if (!jugador.estaMuerto && enemigoActual != null)
        {
            teclasForzadasTutorial = new List<KeyCode> { KeyCode.W };
            if (textoInstrucciones != null)
                textoInstrucciones.text = "¡Excelente! Ahora reacciona con la tecla W.";
            yield return StartCoroutine(RondaDelTutorialInfinita(enemigoActual));
        }

        if (!jugador.estaMuerto && enemigoActual != null)
        {
            teclasForzadasTutorial = new List<KeyCode> { KeyCode.E, KeyCode.R };
            if (textoInstrucciones != null)
                textoInstrucciones.text = "¡Golpe final! Presiona DOS teclas a la vez: [E + R]";
            yield return StartCoroutine(RondaDelTutorialInfinita(enemigoActual));
        }

        if (!jugador.estaMuerto)
        {
            tutorialTerminado = true;
            if (textoInstrucciones != null)
                textoInstrucciones.text = "¡FELICIDADES! Has completado el tutorial de combate. Presiona Enter para comenzar el juego";

            yield return new WaitForSeconds(1.5f);

            if (pngEnter != null) pngEnter.SetActive(true); 
            esperandoEnter = true;
        }
    }

    IEnumerator MostrarPantallaDerrotaTutorial()
    {
        esperandoR = true;
        if (textoInstrucciones != null)
            textoInstrucciones.text = "Has sido derrotado... Practica tu velocidad de reacción.";

        yield return new WaitForSeconds(1.5f);
        if (pngR != null) pngR.SetActive(true); 
    }

    IEnumerator RondaDelTutorialInfinita(Champiñon enemigoActual)
    {
        if (jugador.estaMuerto || enemigoActual == null) yield break;

        posicion.x = -1.05f;
        posicionEnemigo.x = 0.955f;

        yield return StartCoroutine(MoverJugadorTutorial(posicion, 20f));
        yield return StartCoroutine(MoverEnemigoTutorial(enemigoActual, posicionEnemigo, 20f));

        jugador.Transicion();
        yield return new WaitForSeconds(0.3f);
        jugador.Espera();

        GenerarTeclasFijas(enemigoActual);

        float tiempoAntesSenal = enemigoActual.tiempoReaccion * 0.4f;
        yield return new WaitForSeconds(tiempoAntesSenal);

        jugador.Reaccion();
        base.Invoke("ActivarSenal", 0);

        bool acierto = false;
        List<KeyCode> teclasPresionadas = new List<KeyCode>();
        KeyCode[] teclasDisponibles = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };

        while (!acierto && !jugador.estaMuerto)
        {
            foreach (KeyCode tecla in teclasDisponibles)
            {
                if (Input.GetKeyDown(tecla))
                {
                    if (!teclasPresionadas.Contains(tecla)) teclasPresionadas.Add(tecla);
                }
                if (Input.GetKeyUp(tecla))
                {
                    if (teclasPresionadas.Contains(tecla)) teclasPresionadas.Remove(tecla);
                }
            }

            bool todasCorrectas = true;
            foreach (KeyCode tecla in teclasForzadasTutorial)
            {
                if (!teclasPresionadas.Contains(tecla))
                {
                    todasCorrectas = false;
                    break;
                }
            }

            if (todasCorrectas && teclasPresionadas.Count == teclasForzadasTutorial.Count)
            {
                acierto = true;
            }

            yield return null;
        }

        if (jugador.estaMuerto) yield break;

        posicion.x = 0.9f;
        posicionEnemigo.x = -1.033f;
        yield return StartCoroutine(MoverJugadorTutorial(posicion, 20f));
        yield return StartCoroutine(MoverEnemigoTutorial(enemigoActual, posicionEnemigo, 20f));

        jugador.Atacar();
        enemigoActual.Atacar();
        yield return new WaitForSeconds(2f);

        audioSource.PlayOneShot(sonidos[0]);
        enemigoActual.RecibirDanio();
        jugador.Victoria();
        corazonUI.QuitarVidaEnemigo();

        yield return new WaitForSeconds(1f);

        if (enemigoActual.vidasActuales <= 0)
        {
            yield return new WaitForSeconds(2f);
            Destroy(enemigoActual.gameObject);
        }
        else
        {
            enemigoActual.Idle();
        }
        jugador.Idle();

        base.LimpiarRonda();
        yield return new WaitForSeconds(0.8f);
    }

    void GenerarTeclasFijas(Champiñon enemigoActual)
    {
        base.LimpiarRonda();
        KeyCode[] teclasDisponibles = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };

        for (int i = 0; i < teclasDisponibles.Length; i++)
        {
            GameObject nuevaTecla = Instantiate(prefabBase, puntosAparicion[i]);
            nuevaTecla.transform.localPosition = Vector3.zero;

            var listaActivas = (List<GameObject>)GetType().BaseType.GetField("teclasActivas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this);
            listaActivas.Add(nuevaTecla);

            Image img = nuevaTecla.GetComponentInChildren<Image>();
            img.sprite = spritesTeclas[i];
            img.color = Color.white;

            if (teclasForzadasTutorial.Contains(teclasDisponibles[i]))
            {
                var ganadoras = (List<KeyCode>)GetType().BaseType.GetField("teclasGanadoras", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this);
                var correctas = (List<GameObject>)GetType().BaseType.GetField("teclasCorrectas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this);

                ganadoras.Add(teclasDisponibles[i]);
                correctas.Add(nuevaTecla);
            }
        }
    }

    IEnumerator MoverJugadorTutorial(Vector3 destino, float velocidad)
    {
        while (Vector3.Distance(jugador.transform.position, destino) > 0.01f)
        {
            jugador.transform.position = Vector3.MoveTowards(jugador.transform.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }
        jugador.transform.position = destino;
    }

    IEnumerator MoverEnemigoTutorial(Champiñon enemigoTarget, Vector3 destino, float velocidad)
    {
        if (enemigoTarget == null) yield break;
        while (Vector3.Distance(enemigoTarget.transform.position, destino) > 0.01f)
        {
            enemigoTarget.transform.position = Vector3.MoveTowards(enemigoTarget.transform.position, destino, velocidad * Time.deltaTime);
            yield return null;
        }
        enemigoTarget.transform.position = destino;
    }
}