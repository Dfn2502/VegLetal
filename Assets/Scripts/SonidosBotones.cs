using UnityEngine;

public class SonidosBotones : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip[] audioClips;
    public void Play()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
}