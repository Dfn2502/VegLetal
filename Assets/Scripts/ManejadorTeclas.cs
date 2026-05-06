using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManejadorTeclas : MonoBehaviour
{
    public GameObject prefabBase;
    public Sprite[] spritesTeclas; 
    public Transform[] puntosAparicion;
    public float tiempoReaccion = 1.0f;
    public CorazonUI corazonUI;


    public AudioClip[] sonidos;
    public AudioSource audioSource;

    private List<GameObject> teclasActivas = new List<GameObject>();

    private KeyCode teclaGanadora;
    private GameObject teclaCorrecta;

    public Jugador jugador;
    Vector3 posicion;
    Vector3 posicionEnemigo;

    public Champiñon[] enemigos;
    private int indiceEnemigoActual = 0;
    private Champiñon enemigo;

    private KeyCode[] teclas = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };

    void Start()
    {
        posicion = new Vector3(-1.05f,-0.75f,-1.16f);
        posicionEnemigo = new Vector3(0.955f, -0.75f, -1.10186f);

        jugador.transform.position = posicion;

        foreach (var e in enemigos)
            e.gameObject.SetActive(false);

        CargarSiguienteEnemigo();

        corazonUI.MostrarCorazonesJugador(3);
        StartCoroutine(CicloDeJuego());
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

        if (jugador.estaMuerto)
            yield break;

        posicion.x = -1.05f;
        posicionEnemigo.x = 0.955f;
        yield return StartCoroutine(MoverEnemigo(posicionEnemigo, 20f));
        yield return StartCoroutine(MoverJugador(posicion, 20f));
        jugador.Transicion();

        yield return new WaitForSeconds(0.3f);
        
        jugador.Espera();

        GenerarTeclas();

        yield return new WaitForSeconds(1f);

        jugador.Reaccion();
        ActivarSenal();

        float tiempo = 0f;
        bool acierto = false;
        bool inputDetectado = false;

        while (tiempo < tiempoReaccion && !jugador.estaMuerto)
        {
            if (Input.GetKeyDown(KeyCode.Q) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.E) ||
                Input.GetKeyDown(KeyCode.R))
            {
                inputDetectado = true;

                if (Input.GetKeyDown(teclaGanadora))
                {
                    acierto = true;
                }

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
            corazonUI.QuitarVidaEnemigo();
            if (enemigo.vidasActuales <= 0)
            {
                yield return new WaitForSeconds(2f);

                enemigo.gameObject.SetActive(false);

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
            Debug.Log("Fallaste... era: " + teclaGanadora);
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
        int cantidad = Random.Range(1, 5);
        int indiceCorrecto = Random.Range(0, cantidad);

        for (int i = 0; i < cantidad; i++)
        {
            GameObject nuevaTecla = Instantiate(prefabBase, puntosAparicion[i]);
            nuevaTecla.transform.localPosition = Vector3.zero;
            teclasActivas.Add(nuevaTecla);

            int randomIndex = Random.Range(0, teclas.Length);

            KeyCode teclaActual = teclas[randomIndex];
            Sprite spriteActual = spritesTeclas[randomIndex];

            Image img = nuevaTecla.GetComponentInChildren<Image>();
            img.sprite = spriteActual;

            if (i == indiceCorrecto)
            {
                teclaGanadora = teclaActual;
                teclaCorrecta = nuevaTecla;

                Debug.Log("Tecla ganadora: " + teclaGanadora);

                img.color = Color.white; 
            }
            else
            {
                img.color = Color.white;
            }
        }
    }

    void ActivarSenal()
    {

        foreach (GameObject t in teclasActivas)
        {
            Image img = t.GetComponentInChildren<Image>();

            if (t == teclaCorrecta)
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
    void CargarSiguienteEnemigo()
    {
        if (indiceEnemigoActual >= enemigos.Length)
        {
            Debug.Log("GANASTE TODO");
            return;
        }

        if (enemigos == null || enemigos.Length == 0)
        {
            Debug.LogError("No hay enemigos asignados en el Inspector");
            return;
        }

        enemigo = enemigos[indiceEnemigoActual];

        if (enemigo == null)
        {
            Debug.LogError("El enemigo en el índice " + indiceEnemigoActual + " es NULL");
            return;
        }

        enemigo.gameObject.SetActive(true);
        corazonUI.MostrarCorazonesEnemigo(enemigo.vidasMaximas);

        posicionEnemigo.x = 0.955f;
        enemigo.transform.position = posicionEnemigo;
        indiceEnemigoActual++;
    }

    void LimpiarRonda()
    {
        foreach (GameObject t in teclasActivas)
        {
            if (t != null) Destroy(t);
        }

        teclasActivas.Clear();
    }
}