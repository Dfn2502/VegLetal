using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public static CambioEscena Instance;

    public Animator transitionAnimator;
    private string escenaDestino;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CambiarEscena(string nombreEscena)
    {
        escenaDestino = nombreEscena;
        StartCoroutine(SceneLoad());
    }

    public IEnumerator SceneLoad()
    {
        if (transitionAnimator != null)
            transitionAnimator.SetTrigger("StartTransition");

        yield return new WaitForSeconds(1.1f);

        SceneManager.LoadScene(escenaDestino);
    }
}