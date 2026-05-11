using UnityEngine;

public class SpriteDanio : MonoBehaviour
{
    public ParticleSystem hitParticles;

    public void MostrarParticulasDanio()
    {
        if (hitParticles != null)
        {
            hitParticles.Play();
        }
    }
}
