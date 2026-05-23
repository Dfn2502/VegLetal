using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transicion : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 1f;

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(CargarEscena(nombreEscena));
    }

    IEnumerator CargarEscena(string nombreEscena)
    {
        transitionAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(nombreEscena);
    }
}