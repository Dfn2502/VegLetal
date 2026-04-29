using UnityEngine;

public class Champiñon : MonoBehaviour
{
    private Animator componenteAnimator;
    public AudioClip[] sonidos;
    public AudioSource audioSource;

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

        audioSource.PlayOneShot(sonidos[3]);
        componenteAnimator.SetInteger("Estado", 2);
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

    void Update()
    {
        
    }
}
