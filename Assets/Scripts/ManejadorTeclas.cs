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


    public AudioClip[] sonidos;
    public AudioSource audioSource;

    private List<GameObject> teclasActivas = new List<GameObject>();

    private KeyCode teclaGanadora;
    private GameObject teclaCorrecta;

    public Jugador jugador;
    Vector3 posicion;
    Animator animator;

    private KeyCode[] teclas = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>();
        animator = jugador.GetComponentInChildren<Animator>();
        StartCoroutine(CicloDeJuego());
        Debug.Log($"Objeto de script"+ this.gameObject.name);
        posicion = new Vector3(-1.05f,-0.75f,-1.16f);
        jugador.transform.position = posicion;
    }

    IEnumerator CicloDeJuego()
    {
        while (true)
        {
            yield return StartCoroutine(Ronda());
        }
    }

    IEnumerator Ronda()
    {

        posicion.x = -1.05f;
        jugador.transform.position = posicion;
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

        while (tiempo < tiempoReaccion)
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
        jugador.transform.position = posicion;
        jugador.Atacar();
        yield return new WaitForSeconds(2f);
        if (inputDetectado && acierto)
        {
            audioSource.PlayOneShot(sonidos[0]);
            jugador.Idle();


        }
        else
        {
            Debug.Log("Fallaste... era: " + teclaGanadora);
            audioSource.PlayOneShot(sonidos[1]);
            jugador.RecibirDanio();
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
   
    void LimpiarRonda()
    {
        foreach (GameObject t in teclasActivas)
        {
            if (t != null) Destroy(t);
        }

        teclasActivas.Clear();
    }
}