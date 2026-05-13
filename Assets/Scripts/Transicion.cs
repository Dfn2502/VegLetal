using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class Transicion : MonoBehaviour
{
    private Animator transitionAnimator;
    public float transitionTime = 1f;

    private void Start()
    {
        transitionAnimator = GetComponentInChildren<Animator>();
    }

    public IEnumerator CargarEscena(int indiceEscena)
    {
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(indiceEscena);
    }
}
