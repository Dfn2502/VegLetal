using System.Collections;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    private Animator componenteAnimator;
    public ParticleSystem hitParticles;
    public AudioClip[] sonidos;
    public AudioSource audioSource;
    private bool yaEnEspera = false;
    CorazonUI corazonUI;

    public CambioEscena cambioEscena;
    public int vidas;
    public bool estaMuerto = false;

    void Start()
    {
        componenteAnimator = GetComponentInChildren<Animator>();
        corazonUI = FindObjectOfType<CorazonUI>();
        vidas = 3;

    }

    public void Idle()
    {
        componenteAnimator.SetInteger("Estado", 0);
    }

    public void Transicion()
    {
        componenteAnimator.SetInteger("Estado", 1);
    }

    public void Espera()
    {
        if (yaEnEspera) return;

        yaEnEspera = true;


        audioSource.PlayOneShot(sonidos[3]);
        componenteAnimator.SetInteger("Estado", 2);

        StartCoroutine(ResetEspera());
    }

    IEnumerator ResetEspera()
    {
        yield return new WaitForSeconds(0.5f);
        yaEnEspera = false;
    }

    public void Reaccion()
    {
        componenteAnimator.SetInteger("Estado", 3);
    }

    public void Atacar()
    {
        audioSource.PlayOneShot(sonidos[2]);
        componenteAnimator.SetInteger("Estado", 4);
    }

    public void RecibirDanio()
    {

        corazonUI.QuitarVidaJugador();
        vidas--;
        audioSource.PlayOneShot(sonidos[4]);
        componenteAnimator.SetInteger("Estado", 5);
        if (vidas <= 0)
        {
            Muerte();
        }
    }
    public void Muerte()
    {
        if (estaMuerto) return;
        estaMuerto = true;

        audioSource.PlayOneShot(sonidos[5]);
        componenteAnimator.SetInteger("Estado", 6);

        StartCoroutine(MorirYCambiarEscena());
    }

    public void Victoria()
    {
        componenteAnimator.SetInteger("Estado", 7);
    }
    IEnumerator MorirYCambiarEscena()
    {
        yield return new WaitForSeconds(3.5f);
        PlayerPrefs.SetInt("ScoreFinal", FindAnyObjectByType<ManejadorTeclas>().puntaje);
        cambioEscena.CambiarEscena("GameOver");
    }

    public void MostrarParticulasDanio()
    {
        if (hitParticles != null)
        {
            hitParticles.Play();
        }
    }
}