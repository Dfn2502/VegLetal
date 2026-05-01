using System.Collections;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    private Animator componenteAnimator;
    public AudioClip[] sonidos;
    public AudioSource audioSource;
    private bool yaEnEspera = false;


    void Start()
    {
        componenteAnimator = GetComponentInChildren<Animator>();

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

        Debug.Log("ESPERA REAL");

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
        audioSource.PlayOneShot(sonidos[4]);
        componenteAnimator.SetInteger("Estado", 5);
    }
}
