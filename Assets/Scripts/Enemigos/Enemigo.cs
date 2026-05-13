using UnityEngine;

public class Champiñon : MonoBehaviour
{
    private Animator componenteAnimator;
    public AudioClip[] sonidos;
    public AudioSource audioSource;
    public ParticleSystem hitParticles;
    public int vidasMaximas = 3;
    public int vidasActuales;
    public float tiempoReaccion = 1f;
    public bool muerto = false;
    void Start()
    {
        componenteAnimator = GetComponentInChildren<Animator>(true);
        vidasActuales = vidasMaximas;
    }

    public void Idle()
    {
        if(muerto) return;
        componenteAnimator.SetInteger("Estado", 0);
    }


    public void Atacar()
    {
        if (muerto) return;

        componenteAnimator.SetInteger("Estado", 1);
    }

    public void RecibirDanio()
    {

        MostrarParticulasDanio();
        if (muerto) return;

        vidasActuales--;
        componenteAnimator.SetInteger("Estado", 2);
        if(vidasActuales <= 0)
        {
            Muerte();
        }
    }
    public void Muerte()
    {
        if (muerto) return;
        componenteAnimator.SetInteger("Estado", 3); 
    }

    public void MostrarParticulasDanio()
    {
        if (hitParticles != null)
        {
            hitParticles.Play();
        }
    }

}
